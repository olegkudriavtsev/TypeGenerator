namespace ObjectGenerator.TestObjects
{
    public class NestedType
    {
        private string _privateField;

        private string _privateFieldWithPublicProperty;
        
        public string PublicReadWriteProperty { get => _privateFieldWithPublicProperty; set => _privateFieldWithPublicProperty = value; }
        
        public string PublicReadOnlyProperty { get; set; }

        public SimpleObject Object { get; set; }
    }
}
