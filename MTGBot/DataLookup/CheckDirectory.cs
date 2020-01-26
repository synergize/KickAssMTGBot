using MTGBot.APICredentials;
using System;
using System.IO;
using System.Net;

namespace MTGBot.DataLookup
{
    static class CheckDirectory
    {
        public static void GetCheckDirectory()
        {
            Console.WriteLine("Checking if our Data Directory Exists.");
            if (!Directory.Exists(APIObject.DataDirectoryPath))
            {
                //If the directory for our basecode json file doesn't exist we create it along with the json file. 
                DirectoryInfo dir = Directory.CreateDirectory(APIObject.DataDirectoryPath);
                Console.WriteLine("Directory didn't exists. It's been constructed.");
            }
            Console.WriteLine("Checking if our data base exists.");
            if (!CodeExists())
            {
                Console.WriteLine("Data data non existing. Downloading.");
                //if the directory exists but the file doesn't, we create the file. 
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://mtgjson.com/json/AllSets.sqlite", $"{APIObject.BuildFilePath(APIObject.MTGDataBase)}");
                }
            }
        }
        private static bool CodeExists()
        {
            return File.Exists(APIObject.BuildFilePath(APIObject.MTGDataBase));
        }
    }
}
