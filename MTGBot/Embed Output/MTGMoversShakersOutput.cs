using Discord;
using System.Collections.Generic;
using WebScraping.Data_Models;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
        public static EmbedBuilder GetDailyIncreaseMoversOutput(List<MoverCardDataModel> cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = "Daily Price Winners!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }
        public static EmbedBuilder GetDailyDecreaseMoversOutput(List<MoverCardDataModel> cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = "Daily Price Losers!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }
        public static EmbedBuilder GetWeeklyDecreaseMoversOutput(List<MoverCardDataModel> cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = "Weekly Price Losers!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }
        public static EmbedBuilder GetWeeklyIncreaseMoversOutput(List<MoverCardDataModel> cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = "Weekly Price Winners!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }
    }
}
