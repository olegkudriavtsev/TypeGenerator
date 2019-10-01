using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ObjectGenerator
{
    public class MockTypeGenerator
    {
        public List<T> GenerateDefault<T>(bool allowNull)
        {
            var combinations = new List<T>();

            var instance = (T)Activator.CreateInstance(typeof(T));

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => prop.CanRead && prop.CanWrite).ToArray();

            FillProperties(properties, instance);

            if (typeof(T).BaseType != null)
            {
                // todo: add logic for working with base type fields
                var fields = typeof(T).BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            }

            combinations.AddRange(CombinationsOf(instance, allowNull));

            return combinations;
        }

        private void FillProperties(PropertyInfo[] properties, object instance)
        {
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    var defaultTypeValue = GetDefaultTypeValue(property.PropertyType);
                    property.SetValue(instance, defaultTypeValue);
                }
                else
                {
                    var propertyInstance = Activator.CreateInstance(property.PropertyType);
                    FillProperties(
                        property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(prop => prop.CanRead && prop.CanWrite).ToArray(), propertyInstance);
                    property.SetValue(instance, propertyInstance);
                }
            }
        }

        private object GetDefaultTypeValue(Type type)
        {
            if (IsNumericType(type))
            {
                return 1;
            }

            if (Type.GetTypeCode(type) == TypeCode.String)
            {
                return "hello";
            }

            return null;
        }

        private bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private List<T> CombinationsOf<T>(T template, bool allowNull)
        {
            var properties = typeof(T).GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToArray();

            var combinations = 1 << properties.Length;
            var result = new List<T>(combinations - 1);
            for (var i = 1; i < combinations; i++)
            {
                var instance = (T)Activator.CreateInstance(typeof(T));
                var bits = i;
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                    {
                        property.SetValue(instance, bits % 2 == 1 ? property.GetValue(template) : property.PropertyType.IsValueType && allowNull ? Activator.CreateInstance(property.PropertyType) : null);
                    }
                    else
                    {
                        var method = typeof(MockTypeGenerator).GetMethod("CombinationsOf", BindingFlags.NonPublic | BindingFlags.Instance);
                        var generic = method.MakeGenericMethod(property.PropertyType);
                        var nestedObjectCombinations = generic.Invoke(this, new[] { property.GetValue(template) });

                        if (nestedObjectCombinations is IEnumerable asEnumerable)
                        {
                            foreach (var obj in asEnumerable)
                            {
                                var clonedObject = instance.CloneJson();

                                clonedObject.GetType().GetProperty(property.Name).SetValue(clonedObject, obj);
                                result.Add(clonedObject);
                            }
                        }
                    }

                    bits = bits >> 1;
                }

                result.Add(instance);
            }

            return result;
        }
    }
}
