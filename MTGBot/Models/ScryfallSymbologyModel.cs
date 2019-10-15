using Newtonsoft.Json;
using System.Collections.Generic;


namespace MTGBot.Models
{
    public class ScryfallSymbologyModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("svg_uri")]
        public string Svg_uri { get; set; }
    }

    public class Symbology
    {
        public List<ScryfallSymbologyModel> Data { get; set; }
    }
}
