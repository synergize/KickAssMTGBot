﻿using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using VTFileSystemManagement;

namespace MTGBot.Data.ReadWriteJSON
{
    public static class MoversShakersJSONController
    {
        private const string serverInfoLocation = @"\\DESKTOP-JF26JGH\MoversAndShakersJsonData";
        private const string registeredServerNames = "AllRegisteredServers.json";

        public static DiscordServerChannelModel AddChannelFormat(DiscordServerChannelModel serverInformation, string formatName)
        {
            var listFormats = serverInformation.ListOfFormats ?? new List<string>();

            if (!listFormats.Contains(formatName))
            {
                listFormats.Add(formatName);
            }

            serverInformation.ListOfFormats = listFormats;
            return UpdateServerInfo(serverInformation);
        }

        public static DiscordServerChannelModel RemoveChannelFormat(DiscordServerChannelModel serverInformation, string formatName)
        {
            var listFormats = serverInformation.ListOfFormats ?? new List<string>();

            if (listFormats.Contains(formatName))
            {
                listFormats.Remove(formatName);
            }

            serverInformation.ListOfFormats = listFormats;
            return UpdateServerInfo(serverInformation);
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
            UpdateListOfRegisteredGuilds(serverInformation);
            return serverInformation;
        }

        private static void UpdateListOfRegisteredGuilds(DiscordServerChannelModel serverInformation)
        {
            try
            {
                string FilePath = Path.Combine(serverInfoLocation, registeredServerNames);
                var currentServerInfo = ReadRegisteredDiscordGuilds();

                if (currentServerInfo.ListOfRegisteredDiscordGuilds == null)
                {
                    currentServerInfo.ListOfRegisteredDiscordGuilds = new List<ulong>();
                }

                if (!currentServerInfo.ListOfRegisteredDiscordGuilds.Contains(serverInformation.serverID))
                {
                    currentServerInfo.ListOfRegisteredDiscordGuilds.Add(serverInformation.serverID);
                }

                using (StreamWriter file = File.CreateText(FilePath))
                {
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, currentServerInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static MoversAndShakersServerInfoDataModel ReadRegisteredDiscordGuilds()
        {
            FileSystemManager fileSystem = new FileSystemManager();
            string FilePath = Path.Combine(serverInfoLocation, registeredServerNames);

            if (fileSystem.IsFileExists(registeredServerNames, serverInfoLocation))
            {                
                return JsonConvert.DeserializeObject<MoversAndShakersServerInfoDataModel>(fileSystem.ReadJsonFileFromSpecificLocation(registeredServerNames, serverInfoLocation));
            }
            else
            {
                return new MoversAndShakersServerInfoDataModel
                {
                    ListOfRegisteredDiscordGuilds = new List<ulong>()
                };
            }
        }

        public static DateTime AcquireLastScrapeTime()
        {
            const string serverLocation = @"\\DESKTOP-JF26JGH\MoversShakersScraped";
            const string fileName = @"SuccessfulScrapedTime.json";
            FileSystemManager fileSystem = new FileSystemManager();

            if (fileSystem.IsFileExists(fileName, serverLocation))
            {
                return JsonConvert.DeserializeObject<MoversAndShakersServerInfoDataModel>(fileSystem.ReadJsonFileFromSpecificLocation(fileName, serverLocation)).LastSuccessfulScrape;
            }
            else
            {
                return new MoversAndShakersServerInfoDataModel
                {
                    LastSuccessfulScrape = DateTime.Now

                }.LastSuccessfulScrape;
            }
        }

        public static MoverCardDataModel GetMoverCardScrapedData(string fileName)
        {
            const string serverLocation = @"\\DESKTOP-JF26JGH\MoversShakersScraped";
            FileSystemManager fileSystem = new FileSystemManager();

            if (fileSystem.IsFileExists(fileName, serverLocation))
            {
                return JsonConvert.DeserializeObject<MoverCardDataModel>(fileSystem.ReadJsonFileFromSpecificLocation(fileName, serverLocation));
            }
            else
            {
                return null;
            }
        }
    }
}
