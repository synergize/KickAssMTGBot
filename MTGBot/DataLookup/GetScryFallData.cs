using MTGBot.Helpers;
using MTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace MTGBot.DataLookup
{
    class GetScryFallData
    {
        private static Dictionary<string, ScryfallDataModel.BaseCodeObject> CardDictionary = new Dictionary<string, ScryfallDataModel.BaseCodeObject>();
        public static ScryfallDataModel.BaseCodeObject PullScryfallData(string cardname)
        {
            var PullCard = new ScryfallDataModel.BaseCodeObject(); 
            cardname = FormatUserInput.FormatEntry(cardname);

            if (CardDictionary.ContainsKey(cardname))
            {
                return CardDictionary[cardname];
            }
            //Store downloaded summary into memory. 
            string CallAPI = ExactScryFall(cardname);
            if (CallAPI != null)
            {
                PullCard = JsonConvert.DeserializeObject<ScryfallDataModel.BaseCodeObject>(CallAPI, JsonDeserializeHelper.settings);
                PullCard.AllLegalities = SetLegalList(PullCard.Legalities);
                CardDictionary.Add(cardname, PullCard);
                return PullCard;
            }
            return null;
        }

        public static ScryFallCardRulingsModel PullScryFallRuleData(Guid scryfallId)
        {
            using (var web = new WebClient())
            {
                try
                {
                    var _url = string.Format($"https://api.scryfall.com/cards/{scryfallId}/rulings");
                    string _downloadRules = web.DownloadString(_url);
                    if (_downloadRules != null)
                    {
                        return JsonConvert.DeserializeObject<ScryFallCardRulingsModel>(_downloadRules, JsonDeserializeHelper.settings);
                    }
                    return null;
                }
                catch (WebException msg)
                {
                    Console.WriteLine($"Failed to find ruling data. Reason: {msg.Message}");
                    return null;
                }
            }
        }

        public static ScryfallDataModel.BaseCodeObject PullScryFallRandomCard(bool isCommander)
        {
            using (var web = new WebClient())
            {
                try
                {
                    var _url = string.Format($"https://api.scryfall.com/cards/random{(isCommander ? "?q=is%3Acommander" : "")}" );
                    string _downloadRules = web.DownloadString(_url);
                    if (_downloadRules != null)
                    {
                        var PulledCard = JsonConvert.DeserializeObject<ScryfallDataModel.BaseCodeObject>(_downloadRules, JsonDeserializeHelper.settings);
                        PulledCard.AllLegalities = SetLegalList(PulledCard.Legalities);
                        return PulledCard;
                    }
                    return null;
                }
                catch (WebException msg)
                {
                    Console.WriteLine(msg.Message);
                    return null;
                }
            }
        }

        public static ScryFallAutoCompleteModel PullScryFallAutoComplete(string entry)
        {
            using (var web = new WebClient())
            {
                try
                {
                    var _url = string.Format($"https://api.scryfall.com/cards/autocomplete?q={entry}");
                    string _downloadRules = web.DownloadString(_url);
                    if (_downloadRules != null)
                    {
                        return JsonConvert.DeserializeObject<ScryFallAutoCompleteModel>(_downloadRules, JsonDeserializeHelper.settings);
                    }
                    return null;
                }
                catch (WebException msg)
                {
                    Console.WriteLine(msg.Message);
                    return null;
                }
            }
        }
        public static Symbology PullScryfallSymbology()
        {
            string _downloadNews = null;
            using (var web = new WebClient())
            {
                try
                {
                    var _url = string.Format($"https://api.scryfall.com/symbology");
                    _downloadNews = web.DownloadString(_url);
                }
                catch (WebException msg)
                {
                    Console.WriteLine($"Unable to access symbology API Call \n {msg}");
                    _downloadNews = null;
                }
            }

            if (_downloadNews == null)
                return null;
            return JsonConvert.DeserializeObject<Symbology>(_downloadNews);
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
                    Console.WriteLine(msg.Message);
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
        private static Dictionary<string, string> SetLegalList(ScryfallDataModel.Legalities legalities)
        {
            Dictionary<string, string> LegalitiesDictonary = new Dictionary<string, string>();
            LegalitiesDictonary.Add("Standard", LegalityDictionary.Legality[legalities.Standard]);
            LegalitiesDictonary.Add("Modern", LegalityDictionary.Legality[legalities.Modern]);
            LegalitiesDictonary.Add("Legacy", LegalityDictionary.Legality[legalities.Legacy]);
            LegalitiesDictonary.Add("Vintage", LegalityDictionary.Legality[legalities.Vintage]);
            LegalitiesDictonary.Add("Commander", LegalityDictionary.Legality[legalities.Commander]);
            LegalitiesDictonary.Add("Pauper", LegalityDictionary.Legality[legalities.Pauper]);

            return LegalitiesDictonary;
        }
    }
}
