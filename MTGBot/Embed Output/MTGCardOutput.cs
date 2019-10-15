using Discord;
using MTGBot.Models;
using System.Collections.Generic;

namespace MTGBot.Embed_Output
{
    class MTGCardOutput
    {
        private Color successfulColor = Color.DarkGreen;
        private const int failedColor = 16580608;
        private int customColor = 0;
        private const string successfulFooter = "Powered By Scryfall API. Contact Coaction#5994 for suggestions or bugs";
        private const string failedFooter = "Contact Coaction#5994 for any bugs. This is a work in progress.";
        public MTGCardOutput()
        {

        }

        public MTGCardOutput(int embedColor)
        {
            customColor = embedColor;
        }
        public EmbedBuilder CardOutput(ScryfallDataModel.BaseCodeObject PulledCard)
        {
            EmbedBuilder Card = new EmbedBuilder();

            Card.ThumbnailUrl = PulledCard.ImageUris.Png ?? "none";            
            
            if (PulledCard.Power == null || PulledCard.Toughness == null)
            {
                Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText}");
            }
            else if (!PulledCard.Power.Contains("*"))
            {
                Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText} \n {PulledCard.Power}/{PulledCard.Toughness}");
            }
            else
            {
                Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText} \n \\{PulledCard.Power}/{PulledCard.Toughness}");
            }
            Card.WithColor(successfulColor);
            Card.Url = PulledCard.ScryfallUri;
            Card.Title = $"{PulledCard.Name} {PulledCard.ManaCost}";
            Card.Fields = DetermineLegality(PulledCard);

            Card.AddField("Non-Foil Price: ", $"{PulledCard.Prices.Usd ?? "No Pricing Data".TrimStart('$')}", false);
            Card.AddField("Foil Price: ", $"{PulledCard.Prices.UsdFoil ?? "No Pricing Data".TrimStart('$')}", false);

            Card.WithFooter(successfulFooter);

            return Card;
        }

        public EmbedBuilder RulingOutput(ScryFallCardRulingsModel rulings, ScryfallDataModel.BaseCodeObject cardData)
        {
            EmbedBuilder rulingEmbed = new EmbedBuilder();
            const int dataCount = 9; // This limit is added due to Discord's hard limitation on message character sizes. 
            rulingEmbed.WithColor(successfulColor);
            rulingEmbed.Title = $"Rulings for {cardData.Name}";
            rulingEmbed.Url = cardData.ScryfallUri;

            if (rulings.data.Count > dataCount)
            {
                for (int i = 0; i < dataCount; i++)
                {
                    rulingEmbed.AddField($"{rulings.data[i].published_at}", rulings.data[i].comment, false);
                }
                rulingEmbed.AddField($"Additional Rulings", $"This card had too many rules to fit. Plesae check out the rest of them on ScryFall \n {cardData.ScryfallUri}", false);
            }
            else
            {
                foreach (var rule in rulings.data)
                {
                    rulingEmbed.AddField($"{rule.published_at}", rule.comment, false);
                }
            }

            return rulingEmbed;
        }

        private EmbedBuilder APISpellFailure(ScryFallAutoCompleteModel entry)
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(failedColor);
            MTGFailure.WithFooter(failedFooter);
            MTGFailure.AddField("Incorrect Spelling: ", "Make sure to correctly type the name of the card you'd like to look up.", true);
            if (entry != null)
            {
                string names = "";
                foreach (var name in entry.data)
                {
                    names += $"{name}\n";
                }
                MTGFailure.AddField("Did You Mean?", names);
            }
            return MTGFailure;
        }

        private EmbedBuilder APIMultipleEntryFailure()
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(failedColor);
            MTGFailure.WithFooter(failedFooter);
            MTGFailure.AddField("Too many card look ups: ", "Please make sure to only link to one card per message.", true);

            return MTGFailure;
        }

        private EmbedBuilder GenericError()
        {
            EmbedBuilder MTGFailure = new EmbedBuilder();
            MTGFailure.Title = "Card Lookup Failed.";
            MTGFailure.WithColor(failedColor);
            MTGFailure.WithFooter(failedFooter);
            MTGFailure.AddField("Whoopsies! ", "I'm not sure what exploded, but something did. Please contact my owner.", true);

            return MTGFailure;
        }

        public EmbedBuilder DetermineFailure(int num, ScryFallAutoCompleteModel entry = null)
        {
            switch (num)
            {
                default:
                    return GenericError();
                case 0:
                    return APISpellFailure(entry);
            }
        }

        public EmbedBuilder GettingHelp()
        {
            EmbedBuilder Helping = new EmbedBuilder();
            Helping.Title = "Help Center";
            Helping.Description = "Below is a list of commands and features of this bot! It's a work in progress.";
            Helping.AddField("Card Lookup: ", "Cards can be located with double open and closing brackets [[like this]]. They can be anywhere in your sentence!");
            Helping.WithFooter(successfulFooter);
            Helping.WithColor(successfulColor);

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

            if (Legal.Count > 0 && Legal != null)
            {
                EmbededList.Add(BuildLegalityFields("Legal", EmbedLegalityStringBuilder(Legal)));
            }
            if (NotLegal.Count > 0 && NotLegal != null)
            {
                EmbededList.Add(BuildLegalityFields("Not Legal", EmbedLegalityStringBuilder(NotLegal)));
            }
            if (Banned.Count > 0 && Banned != null)
            {
                EmbededList.Add(BuildLegalityFields("Banned", EmbedLegalityStringBuilder(Banned)));
            }
            if (Restricted.Count > 0 && Restricted != null)
            {
                EmbededList.Add(BuildLegalityFields("Restricted", EmbedLegalityStringBuilder(Restricted)));
            }

            return EmbededList;
        }

        private List<string> FillLegalitiesLists(Dictionary<string, string> legals, string expectedLegality)
        {
            if (legals == null)
                return null;

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
