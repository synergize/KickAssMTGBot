using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
