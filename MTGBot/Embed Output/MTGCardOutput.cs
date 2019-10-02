using Discord;
using MTGBot.DataLookup;
using MTGBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            Card.WithDescription($"{PulledCard.TypeLine} \n {PulledCard.OracleText} \n {PulledCard.Power}/{PulledCard.Toughness}");
            Card.WithColor(4124426);
            Card.Url = PulledCard.ScryfallUri;
            Card.Title = $"{PulledCard.Name} {PulledCard.ManaCost}";
            //Card.AddField("Standard: ", LegalityDictionary.Legality[PulledCard.Legalities.Standard], true);
            //Card.AddField("Modern: ", LegalityDictionary.Legality[PulledCard.Legalities.Modern], true);
            //Card.AddField("Legacy: ", LegalityDictionary.Legality[PulledCard.Legalities.Legacy], true);
            //Card.AddField("Vintage: ", LegalityDictionary.Legality[PulledCard.Legalities.Vintage], true);
            DetermineLegality(Card, PulledCard);
            if (PulledCard.Prices.Usd != null)
            {
                Card.AddField("Non-Foil Price: ", $"${PulledCard.Prices.Usd}", true);
            }
            if (PulledCard.Prices.UsdFoil != null)
            {
                Card.AddField("Foil Price: ", $"${PulledCard.Prices.UsdFoil}", true);
            }
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

        private EmbedBuilder DetermineLegality(EmbedBuilder embedOutput, ScryfallDataModel.BaseCodeObject cardInfo)
        {
            
            return null; 
        }
    }
}
