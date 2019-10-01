using System.Collections.Generic;

namespace ObjectGenerator.TestObjects
{
    public class MainType
    {
        private string _privateField;

        private string _privateFieldWithPublicProperty;

        private NestedType _privateNestedType;

        private List<NestedType> _privateNestedTypeCollection;

        private Dictionary<string, NestedType> _privateDictionaryWithNestedType;
        
        public string PublicReadWriteProperty { get; set; }
        
        public string PublicReadOnlyProperty { get; }
        
        public NestedType PublicNestedTypeProperty { get; set; }
        
        public string PublicFieldForPrivateProperty
        {
            get => _privateFieldWithPublicProperty;
            set => _privateFieldWithPublicProperty = value;
        }
    }
}
