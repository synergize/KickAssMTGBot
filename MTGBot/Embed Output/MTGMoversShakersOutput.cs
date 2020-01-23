using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers.Enums;
using System;
using System.Threading.Tasks;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
        private EmbedBuilder GetDailyIncreaseMoversOutput(MoverCardDataModel cardsList)
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

        private EmbedBuilder GetDailyDecreaseMoversOutput(MoverCardDataModel cardsList)
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

        private EmbedBuilder GetWeeklyDecreaseMoversOutput(MoverCardDataModel cardsList)
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

        private EmbedBuilder GetWeeklyIncreaseMoversOutput(MoverCardDataModel cardsList)
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

        public async Task DeliverMoversOutputAsync(DiscordSocketClient Client)
        {
            var registeredGuilds = MoversShakersJSONController.ReadRegisteredDiscordGuilds();

            foreach (var guild in registeredGuilds.ListOfRegisteredDiscordGuilds)
            {
                var guildConfig = MoversShakersJSONController.ReadMoversShakersConfig(guild);

                foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
                {
                    var scrapedDailyIncrease = MoversShakersJSONController.GetMoverCardScrapedData($"DailyIncrease_{formatName.ToString()}.json");
                    var scrapedDailyDecrease = MoversShakersJSONController.GetMoverCardScrapedData($"DailyDecrease_{formatName.ToString()}.json");

                    var scrapedWeeklyIncrease = MoversShakersJSONController.GetMoverCardScrapedData($"WeeklyIncrease_{formatName.ToString()}.json");
                    var scrapedWeeklyDecrease = MoversShakersJSONController.GetMoverCardScrapedData($"WeeklyDecrease_{formatName.ToString()}.json");

                    foreach (var item in guildConfig.ListOfFormats)
                    {
                        if (item == formatName.ToString())
                        {
                            
                            try
                            {
                                var channel = Client.GetGuild(guildConfig.serverID).GetChannel(guildConfig.channelID) as IMessageChannel;
                                await channel.SendMessageAsync("", false, GetDailyIncreaseMoversOutput(scrapedDailyIncrease).Build());
                                await channel.SendMessageAsync("", false, GetDailyDecreaseMoversOutput(scrapedDailyDecrease).Build());
                                await channel.SendMessageAsync("", false, GetWeeklyIncreaseMoversOutput(scrapedWeeklyIncrease).Build());
                                await channel.SendMessageAsync("", false, GetWeeklyDecreaseMoversOutput(scrapedWeeklyDecrease).Build());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }
                    }
                }
            }

        }
    }
}
