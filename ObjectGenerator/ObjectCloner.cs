using Newtonsoft.Json;

namespace ObjectGenerator
{
    public static class ObjectCloner
    {
        public static T CloneJson<T>(this T source)
        {            
            if (ReferenceEquals(source, null))
            {
                return default;
            }

            var deserializeSettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}
