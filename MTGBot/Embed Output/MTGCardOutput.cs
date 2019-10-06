using Discord;
using MTGBot.DataLookup;
using MTGBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.IO;

namespace MTGBot.Embed_Output
{
    class MTGCardOutput
    {
        public EmbedBuilder CardOutput(ScryfallDataModel.BaseCodeObject PulledCard)
        {
            EmbedBuilder Card = new EmbedBuilder();
            if (PulledCard.ImageUris.Png != null)
            {
                Card.ThumbnailUrl = PulledCard.ImageUris.Png;
            }
            if (PulledCard.Power == null || PulledCard.Toughness == null)
            {
                Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText}");
            }
            else
            {
               Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText} \n \\{PulledCard.Power}/{PulledCard.Toughness}");
            }
            Card.WithColor(4124426);
            Card.Url = PulledCard.ScryfallUri;
            Card.Title = $"{PulledCard.Name} {PulledCard.ManaCost}";
            Card.Fields = DetermineLegality(PulledCard);

            Card.AddField("Non-Foil Price: ", $"{PulledCard.Prices.Usd ?? "No Pricing Data".TrimStart('$')}", false);        
            Card.AddField("Foil Price: ", $"{PulledCard.Prices.UsdFoil ?? "No Pricing Data".TrimStart('$')}", false);
            
            Card.WithFooter("Powered By scryfall API. Contact Coaction#5994 for suggestions or issues");

            return Card;
        }

        private EmbedBuilder APISpellFailure()
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(16580608);
            MTGFailure.WithFooter("Contact Coaction#5994 for any issues. This is a work in progress.");
            MTGFailure.AddField("Incorrect Spelling: ", "Make sure to correctly type the name of the card you'd like to look up.", true);

            return MTGFailure;
        }

        private EmbedBuilder APIMultipleEntryFailure()
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(16580608);
            MTGFailure.WithFooter("Contact Coaction#5994 for any issues. This is a work in progress.");
            MTGFailure.AddField("Too many card look ups: ", "Please make sure to only link to one card per message.", true);

            return MTGFailure;
        }

        private EmbedBuilder GenericError()
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(16580608);
            MTGFailure.WithFooter("Contact Coaction#5994 for any issues. This is a work in progress.");
            MTGFailure.AddField("Whoopsies! ", "I'm not sure what exploded, but something did. Please contact my owner.", true);

            return MTGFailure;
        }

        public EmbedBuilder DetermineFailure(int num)
        {
            switch (num)
            {
                default:
                    return GenericError();
                case 0:
                    return APISpellFailure();
                case 1:
                    return APIMultipleEntryFailure();
            }
        }

        public EmbedBuilder GettingHelp()
        {
            EmbedBuilder Helping = new EmbedBuilder();
            Helping.Title = "Help Center";
            Helping.Description = "Below is a list of commands and features of this bot! It's a work in progress.";
            Helping.AddField("Card Lookup: ", "Cards can be located with double open and closing brackets [[like this]]. They can be anywhere in your sentence!");
            Helping.WithFooter("Contact Coaction#5994 for any issues or ideas. This is a work in progress.");
            Helping.WithColor(4124426);

            return Helping;
        }

        private List<EmbedFieldBuilder> DetermineLegality(ScryfallDataModel.BaseCodeObject cardInfo)
        {
            List<EmbedFieldBuilder> EmbededList = new List<EmbedFieldBuilder>();
            var dictionary = cardInfo.AllLegalities;
            var NotLegal = FillLegalitiesLists(dictionary, "Not Legal");
            var Legal = FillLegalitiesLists(dictionary, "Legal");
            var Banned = FillLegalitiesLists(dictionary, "Banned");
            var Restricted = FillLegalitiesLists(dictionary, "Restricted");

            if (Legal.Count > 0)
            {
                EmbededList.Add(BuildLegalityFields("Legal", EmbedLegalityStringBuilder(Legal)));
            }
            if (NotLegal.Count > 0)
            {
                EmbededList.Add(BuildLegalityFields("Not Legal", EmbedLegalityStringBuilder(NotLegal)));
            }
            if (Banned.Count > 0)
            {
                EmbededList.Add(BuildLegalityFields("Banned", EmbedLegalityStringBuilder(Banned)));
            }
            if (Restricted.Count > 0)
            {
                EmbededList.Add(BuildLegalityFields("Restricted", EmbedLegalityStringBuilder(Restricted)));
            }

            return EmbededList;
        }

        private List<string> FillLegalitiesLists(Dictionary<string, string> legals, string expectedLegality)
        {
            List<string> legalities = new List<string>();
            foreach (var x in legals)
            {
                if (x.Value == expectedLegality)
                {
                    legalities.Add(x.Key);
                }
            }
            return legalities;
        }

        private EmbedFieldBuilder BuildLegalityFields(string name, string formats)
        {
            EmbedFieldBuilder buildField = new EmbedFieldBuilder();
            buildField.Name = name;
            if (formats != "")
            {
                buildField.Value = formats;
            }
            buildField.IsInline = true;

            return buildField;
        }

        private string EmbedLegalityStringBuilder(List<string> listOfStrings)
        {
            string output = "";
            foreach (var x in listOfStrings)
            {
                output += $" {x},";
            }
            output.Trim();
            return output.TrimEnd(',');
        }
    }

}
