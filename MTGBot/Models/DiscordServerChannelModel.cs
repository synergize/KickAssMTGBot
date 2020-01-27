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
        public DateTime LastDeliveredTime_Standard { get; set; }
        public DateTime LastDeliveredTime_Modern { get; set; }
        public DateTime LastDeliveredTime_Pioneer { get; set; }
        public DateTime LastDeliveredTime_Pauper { get; set; }
        public DateTime LastDeliveredTime_Legacy { get; set; }
        public DateTime LastDeliveredTime_Vintage { get; set; }

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
