using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.Models
{
    public class ScryfallSymbologyModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("svg_uri")]
        public string Svg_uri { get; set; }
    }

    public class RootObject
    {
        public List<ScryfallSymbologyModel> Data { get; set; }
    }
}
