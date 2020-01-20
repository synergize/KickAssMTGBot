using Discord;
using MoversAndShakersScrapingService.Data_Models;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
        public static EmbedBuilder GetDailyIncreaseMoversOutput(MoverCardDataModel cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Daily Price Winners for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.ListOfCards)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        public static EmbedBuilder GetDailyDecreaseMoversOutput(MoverCardDataModel cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Daily Price Losers for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.ListOfCards)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        public static EmbedBuilder GetWeeklyDecreaseMoversOutput(MoverCardDataModel cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Weekly Price Losers for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.ListOfCards)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        public static EmbedBuilder GetWeeklyIncreaseMoversOutput(MoverCardDataModel cardsList)
        {
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Weekly Price Winners for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.ListOfCards)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }
    }
}
