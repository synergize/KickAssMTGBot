using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MoversAndShakersScrapingService.Data_Models
{
    public class MoversAndShakersServerInfoDataModel
    {
        [JsonProperty("ConfiguredDiscordGuilds")]
        public List<ulong> ListOfRegisteredDiscordGuilds { get; set; }
        public DateTime LastSuccessfulScrape { get; set; }
    }
}
