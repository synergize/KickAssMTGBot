using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTGBot.APICredentials
{
    public static class APIObject
    {
        private static string PuKey = "97BFE570-A33C-41E0-84FD-5DE73853D81C";
        private static string PrKey = "28496FF6-D601-4A60-8BF6-8DF01D5C3D6D";
        private static string Vrsn = "v1.20.0";
        private static string DataDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Data");
        private static string MTGCardsFile = @"AllMTGCards.json";
        private static string SQLDatabase = @"AllSets.sqlite";

        public static string PublicKey
        {
            get { return PuKey; } 
        }
        public static string PrivateKey
            {
                get { return PrKey; }
            }
        public static string Version
        {
            get { return Vrsn; }
        }
        public static string DataDirectoryPath
        {
            get { return DataDirectory; }
        }

        public static string MTGFilePath
        {
            get { return MTGCardsFile; }
        }
        public static string MTGDataBase
        {
            get { return SQLDatabase; }
        }
        public static string BuildFilePath(string file)
        {
            return Path.Combine(DataDirectoryPath, file);
        }
    }
}
