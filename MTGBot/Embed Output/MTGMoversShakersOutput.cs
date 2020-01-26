using Discord;
using Discord.WebSocket;
using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers;
using MTGBot.Helpers.Enums;
using System;
using System.Threading.Tasks;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
        public static DateTime MoversShakersTimeStamp = DateTime.MinValue;
        private Color successfulColor = Color.DarkGreen;
        private const int failedColor = 16580608;
        private const string footerMessage = "Contact Coaction#5994 for any bugs. This is a work in progress.";
        private EmbedBuilder GetDailyIncreaseMoversOutput(MoverCardDataModel cardsList)
        {
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Daily Price Winners for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.DailyIncreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        private EmbedBuilder GetDailyDecreaseMoversOutput(MoverCardDataModel cardsList)
        {
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Daily Price Losers for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.DailyDecreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        private EmbedBuilder GetWeeklyDecreaseMoversOutput(MoverCardDataModel cardsList)
        {
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Weekly Price Losers for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.WeeklyDecreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        private EmbedBuilder GetWeeklyIncreaseMoversOutput(MoverCardDataModel cardsList)
        {
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.Title = $"Weekly Price Winners for {cardsList.Format}!";
            BuildEmbed.WithColor(000000);
            foreach (var item in cardsList.WeeklyIncreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        public EmbedBuilder NoConfiguredServerErrorOutput()
        {
            return new EmbedBuilder
            {
                Title = "Configuration Error",
                Description = "Format modification failed due to lack of configuration. Please type !mtgsetchannel #<channel name> then try again.",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = new Color(failedColor)
            };
        }

        public EmbedBuilder IncorrectOrUnspportedFormatError()
        {
            return new EmbedBuilder
            {
                Title = "Format Entry Error",
                Description = "Format was unable to be accepted. This was due to unsupported format or a typo. Please double check your entry and try again. Supported formats are located here: [MTG Gold Fish Movers And Shakers](https://www.mtggoldfish.com/movers/standard)",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = new Color(failedColor)
            };
        }

        public EmbedBuilder SetChannelSuccess(ISocketMessageChannel channel)
        {
            return new EmbedBuilder
            {
                Title = "Channel Successfully Updated",
                Description = $"You've successfully set {channel.Name}! If you already have format(s) configured then the channel will update when MTGGoldFish updates. Otherwise, make sure to add a format via !mtgaddformat <formatname>.",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = successfulColor
            };
        }

        public EmbedBuilder AddFormatSuccess(string formatName)
        {
            return new EmbedBuilder
            {
                Title = "Formats Successfully Updated!",
                Description = $"You've successfully added {formatName} to your list of supported formats. You'll see the format display within 30 minutes!",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = successfulColor
            };
        }

        public EmbedBuilder RemoveFormatSuccess(string formatName)
        {
            return new EmbedBuilder
            {
                Title = "Format Successfully Removed",
                Description = $"You've successfully removed {formatName} from your list of supported formats. You'll no longer see it update.",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = successfulColor
            };
        }

        public async Task DeliverMoversOutputAsync(DiscordSocketClient Client)
        {
            var registeredGuilds = MoversShakersJSONController.ReadRegisteredDiscordGuilds();

            foreach (var guild in registeredGuilds.ListOfRegisteredDiscordGuilds)
            {
                var guildConfig = MoversShakersJSONController.ReadMoversShakersConfig(guild);

                foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
                {
                    var scrapedData = MoversShakersJSONController.GetMoverCardScrapedData($"{formatName.ToString()}.json");

                    foreach (var item in guildConfig.ListOfFormats)
                    {
                        if (item == formatName.ToString())
                        {

                            var channel = Client.GetGuild(guildConfig.serverID).GetChannel(guildConfig.channelID) as IMessageChannel;
                            if (scrapedData.DailyIncreaseList.Count > 0)
                            {
                                await channel.SendMessageAsync("", false, GetDailyIncreaseMoversOutput(scrapedData).Build());
                            }
                            if (scrapedData.DailyDecreaseList.Count > 0)
                            {
                                await channel.SendMessageAsync("", false, GetDailyDecreaseMoversOutput(scrapedData).Build());
                            }
                            if (scrapedData.WeeklyIncreaseList.Count > 0)
                            {
                                await channel.SendMessageAsync("", false, GetWeeklyIncreaseMoversOutput(scrapedData).Build());
                            }
                            if (scrapedData.WeeklyDecreaseList.Count > 0)
                            {
                                await channel.SendMessageAsync("", false, GetWeeklyDecreaseMoversOutput(scrapedData).Build());
                            }
                        }
                    }
                }
            }
        }

        public async void DetermineDelivery(DiscordSocketClient Client)
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Started. ###"));
            var serverInformation = MoversShakersJSONController.ReadRegisteredDiscordGuilds();
            var lastScrapeTime = MoversShakersJSONController.AcquireLastScrapeTime();

            foreach (var guild in serverInformation.ListOfRegisteredDiscordGuilds)
            {
                var lastRecordedScrapeTime = MoversShakersJSONController.ReadMoversShakersConfig(guild) ?? new Models.DiscordServerChannelModel
                {
                    LastDeliveredTime = DateTime.MinValue
                };

                Console.WriteLine(ConsoleWriteOverride.AddTimeStamp($"MoversShakersTimeStamp: {MoversShakersTimeStamp.ToString("HH:mm:ss")}"));
                lastRecordedScrapeTime.LastDeliveredTime = DateTime.Now;
                if (MoversShakersTimeStamp != lastScrapeTime && lastScrapeTime != lastRecordedScrapeTime.LastDeliveredTime)
                {
                    MoversShakersTimeStamp = lastScrapeTime;
                    lastRecordedScrapeTime.LastDeliveredTime = lastScrapeTime;
                    MoversShakersJSONController.UpdateServerInfo(lastRecordedScrapeTime);
                    await new MTGMoversShakersOutput().DeliverMoversOutputAsync(Client);

                }
                else if (lastRecordedScrapeTime.LastDeliveredTime == DateTime.MinValue)
                {
                    MoversShakersTimeStamp = lastScrapeTime;
                    lastRecordedScrapeTime.LastDeliveredTime = lastScrapeTime;
                    MoversShakersJSONController.UpdateServerInfo(lastRecordedScrapeTime);
                    await new MTGMoversShakersOutput().DeliverMoversOutputAsync(Client);
                }
            }
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Completed. ###"));
        }
    }
}
