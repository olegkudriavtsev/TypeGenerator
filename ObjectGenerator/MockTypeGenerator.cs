using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectGenerator
{
    public class MockTypeGenerator
    {
        public List<T> CombinationsOf<T>(T template)
        {
            var properties = typeof(T).GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToArray();

            var combinations = 1 << properties.Length;
            var result = new List<T>(combinations - 1);
            for (var i = 1; i < combinations; i++)
            {
                var instance = (T)Activator.CreateInstance(typeof(T));
                var bits = i;
                for (var p = 0; p < properties.Length; p++)
                {
                    properties[p].SetValue(instance, bits % 2 == 1 ? properties[p].GetValue(template) : properties[p].PropertyType.IsValueType ? Activator.CreateInstance(properties[p].PropertyType) : null);
                    bits = bits >> 1;
                }

                var fieldInfos = typeof(T).BaseType.GetFields(
                    BindingFlags.NonPublic | BindingFlags.Instance);

                for (var f = 0; f < fieldInfos.Length; f++)
                {
                    fieldInfos[f].SetValue(instance, bits % 2 == 1 ? properties[f].GetValue(template) : properties[f].PropertyType.IsValueType ? Activator.CreateInstance(properties[f].PropertyType) : null);
                }

                result.Add(instance);
            }

            return result;
        }
    }
}
