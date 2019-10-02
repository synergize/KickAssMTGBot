using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTGBot.Data
{
    public static class FilePathingStaticData
    {
        public static string _DriverDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Driver");
        public static string _DataDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Data");
        public static string BuildFilePathDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return Path.Combine(directory);
        }
        public static string BuildFilePath(string file)
        {
            return Path.Combine(_DataDirectory, file);
        }
    }
}
