using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.DataLookup
{
    class CardDictonary
    {
        public IDictionary<string, JToken> GetDictionary(string file)
        {
            dynamic data = JsonConvert.DeserializeObject(file);
            IDictionary<string, JToken> cards = data;

            return cards;
        }
    }
}
