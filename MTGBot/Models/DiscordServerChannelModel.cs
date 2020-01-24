using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MTGBot.Models
{
    public class DiscordServerChannelModel
    {
        public ulong serverID { get; set; }
        public ulong channelID { get; set; }
        [JsonProperty("ConfiguredFormats", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ListOfFormats { get; set; }
        public DateTime LastDeliveredTime { get; set; }

        public DiscordServerChannelModel(ulong serverid, ulong channelid)
        {
            serverID = serverid;
            channelID = channelid;
        }

        public DiscordServerChannelModel()
        {

        }
    }
}
