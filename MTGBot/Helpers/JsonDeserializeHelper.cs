using Newtonsoft.Json;

namespace MTGBot.Helpers
{
    public static class JsonDeserializeHelper
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate
        };
    }
}
