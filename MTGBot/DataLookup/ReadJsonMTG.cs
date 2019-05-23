using MTGBot.APICredentials;
using MTGBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MTGBot.DataLookup
{
    class ReadJsonMTG
    {
        public string ReadFile()
        {
            try
            {
                var file = "";
                using (StreamReader sr = new StreamReader("C:\\Users\\err0r\\source\\repos\\MTGBot\\MTGBot\\Data\\AllMTGCards.json"))
                {
                    file = sr.ReadToEnd();
                    sr.Close();
                }
                return file;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                throw;
            }
        }
    }
}
