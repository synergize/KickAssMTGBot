using MTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VTFileSystemManagement;

namespace MTGBot.Data.ReadWriteJSON
{
    public static class MoversShakersJSONController
    {
        private const string serverInfoLocation = @"\\DESKTOP-JF26JGH\MoversAndShakersJsonData";

        public static void AddChannelFormat(DiscordServerChannelModel serverInformation, string formatName)
        {
            var listFormats = serverInformation.ListOfFormats ?? new List<string>();

            if (!listFormats.Contains(formatName))
            {
                listFormats.Add(formatName);
            }

            serverInformation.ListOfFormats = listFormats;
            UpdateServerInfo(serverInformation);
        }

        public static void RemoveChannelFormat(DiscordServerChannelModel serverInformation, string formatName)
        {
            var listFormats = serverInformation.ListOfFormats ?? new List<string>();

            if (listFormats.Contains(formatName))
            {
                listFormats.Remove(formatName);
            }

            serverInformation.ListOfFormats = listFormats;
            UpdateServerInfo(serverInformation);
        }

        public static DiscordServerChannelModel ReadMoversShakersConfig(ulong serverID)
        {
            FileSystemManager fileSystem = new FileSystemManager();
            var fileName = $"{serverID}.json";

            if (fileSystem.IsFileExists(fileName, serverInfoLocation))
            {
                return JsonConvert.DeserializeObject<DiscordServerChannelModel>(fileSystem.ReadJsonFileFromSpecificLocation(fileName, serverInfoLocation));
            }
            else
            {
                return null;
            }
        }

        public static DiscordServerChannelModel UpdateServerInfo(DiscordServerChannelModel serverInformation)
        {
            string FilePath = Path.Combine(serverInfoLocation, $"{serverInformation.serverID}.json");

            using (StreamWriter file = File.CreateText(FilePath))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(file, serverInformation);
                Console.WriteLine($"{serverInformation.serverID}.json updated successfully.");
            }

            return serverInformation;
        }
    }
}
