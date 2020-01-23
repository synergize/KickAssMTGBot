using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Data;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers.Enums;
using Newtonsoft.Json;
using System;
using VTFileSystemManagement;

namespace MTGBot.DataLookup.MTGGoldFish
{
    public static class MTGGoldfishController
    {
        public static DateTime timeStamp;

        public static bool DetermineIfSendData(ulong serverId)
        {
            var listOfFormats = MoversShakersJSONController.ReadMoversShakersConfig(serverId).ListOfFormats;
            FileSystemManager fileSystem = new FileSystemManager();

            var savedTimeStamp = MoversShakersJSONController.AcquireLastScrapeTime();

            return false;

        }
    }
}
