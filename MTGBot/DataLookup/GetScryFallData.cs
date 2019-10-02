using MTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MTGBot.DataLookup
{
    class GetScryFallData
    {
        private static ScryfallDataModel.BaseCodeObject PullCard = null;
        private static Dictionary<string, ScryfallDataModel.BaseCodeObject> CardDictionary = new Dictionary<string, ScryfallDataModel.BaseCodeObject>();
        GetScryFallData()
        {
           PullCard = new ScryfallDataModel.BaseCodeObject();
        }
        public static ScryfallDataModel.BaseCodeObject PullScryfallData(string cardname)
        {
            cardname = FormatEntry(cardname);
            if (CardDictionary.ContainsKey(cardname))
            {
                return CardDictionary[cardname];
            }
            //Store downloaded summary into memory. 
            string CallAPI = ExactScryFall(cardname);
            if (CallAPI != null)
            {
                PullCard = JsonConvert.DeserializeObject<ScryfallDataModel.BaseCodeObject>(CallAPI);
                var x = PullCard.ImageUris.Small;
                CardDictionary.Add(cardname, PullCard);
            }                        
                return PullCard;            
        }
        private static string FuzzyScryFall(string cardname)
        {
            string _downloadNews = null;
            using (var web = new WebClient())
            {
                try
                {
                    var _url =
                    string.Format($"https://api.scryfall.com/cards/named?fuzzy={cardname}&format=json");
                    _downloadNews = web.DownloadString(_url);
                }
                catch (WebException msg)
                {
                    Console.WriteLine(msg);
                    return null;
                }
            }
            
            return _downloadNews;
        }
        private static string ExactScryFall(string cardname)
        {
            string _downloadNews = null;
            using (var web = new WebClient())
            {
                try
                {
                    var _url =
                    string.Format($"https://api.scryfall.com/cards/named?exact={cardname}&format=json");
                    _downloadNews = web.DownloadString(_url);
                }
                catch (WebException msg)
                {
                    Console.WriteLine("Failed to acquire card information via exact link. Attempting Fuzzy..");
                    return FuzzyScryFall(cardname);
                }
            }
            return _downloadNews;
        }
        private static string FormatEntry(string entry)
        {
            int count = 0;
            entry = entry.TrimStart().TrimEnd().ToUpper();
            entry = entry.Remove(0, 2);
            count = entry.Length - 2; 
            entry = entry.Remove(count, 2);
            if (entry.Contains(" "))
            {
                entry = entry.Replace(" ", "+");
            }

            return entry;
        }
    }
}
