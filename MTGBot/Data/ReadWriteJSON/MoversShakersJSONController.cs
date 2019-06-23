using MTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTGBot.Data.ReadWriteJSON
{
    class MoversShakersJSONController
    {
        public void SaveMoversChannelToJson(ulong channelID, ulong serverID)
        {
            string FilePath = FilePathingStaticData.BuildFilePath($"{serverID}.json");
            var ReadFile = ReadStatsJson(serverID);
            if (ReadFile == null)
            {
                DiscordServerChannelModel newEntry = new DiscordServerChannelModel();
                newEntry.channelID = channelID;
                newEntry.serverID = serverID;
                using (StreamWriter file = File.CreateText(FilePath))
                {
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, newEntry);
                }
            }
            else
            {
                using (StreamWriter file = File.CreateText(FilePath))
                {
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, UpdateServerInfo(ReadFile, channelID));
                }
            }


        }
        public DiscordServerChannelModel ReadStatsJson(ulong serverID)
        {
            DiscordServerChannelModel obj = new DiscordServerChannelModel();
            string FilePath = FilePathingStaticData.BuildFilePath($"{serverID}.json");
            if (CheckFileExists(FilePath))
            {
                obj = JsonConvert.DeserializeObject<DiscordServerChannelModel>(File.ReadAllText(FilePath));

                return obj;
            }
            else
            {
                return null;
            }
        }
        private bool CheckFileExists(string FilePath)
        {
            string newDir = FilePathingStaticData.BuildFilePathDirectory(FilePathingStaticData._DataDirectory);
            if (!Directory.Exists(newDir))
            {
                DirectoryInfo dir = Directory.CreateDirectory(newDir);
            }
            if (!File.Exists(FilePath))
            {
                var SteamIDJson = File.Create(FilePath);
                SteamIDJson.Close();
                return false;
            }
            else
            {
                return true;
            }
        }
        private DiscordServerChannelModel UpdateServerInfo(DiscordServerChannelModel obj, ulong channelID)
        {
            obj.channelID = channelID;
            return obj;
        }
    }
}
