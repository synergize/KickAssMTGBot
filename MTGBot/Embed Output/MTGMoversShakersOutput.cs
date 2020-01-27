using Discord;
using Discord.WebSocket;
using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers;
using MTGBot.Helpers.Enums;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
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

        public async Task DeliverMoversOutputAsync(DiscordSocketClient Client, string format, ulong guild)
        {
            Thread.Sleep(3000);
            var guildConfig = MoversShakersJSONController.ReadMoversShakersConfig(guild);
            var scrapedData = MoversShakersJSONController.GetMoverCardScrapedData($"{format}.json");

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

        private void UpdateScrapeTime(MTGFormatsEnum format)
        {
            var fileLocation = ConfigurationManager.AppSettings.Get("MoversAndShakersScrapedDataLocation");
            VTFileSystemManagement.FileSystemManager fileSystem = new VTFileSystemManagement.FileSystemManager();

            switch (format)
            {
                case MTGFormatsEnum.standard:
                    ScrapeTimes.StandardScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");                    
                    break;
                case MTGFormatsEnum.modern:
                    ScrapeTimes.ModernScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");
                    break;
                case MTGFormatsEnum.pioneer:
                    ScrapeTimes.PioneerScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");
                    break;
                case MTGFormatsEnum.pauper:
                    ScrapeTimes.PauperScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");
                    break;
                case MTGFormatsEnum.legacy:
                    ScrapeTimes.LegacyScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");
                    break;
                case MTGFormatsEnum.vintage:
                    ScrapeTimes.VintageScrapeTime = fileSystem.GetFilesLastModifiedTime(fileLocation, $"{format.ToString()}.json");
                    break;
                default:
                    break;
            }
        }

        private DateTime GetCurrentScrapeTime(MTGFormatsEnum format)
        {
            switch (format)
            {
                case MTGFormatsEnum.standard:
                    return ScrapeTimes.StandardScrapeTime;
                case MTGFormatsEnum.modern:
                    return ScrapeTimes.ModernScrapeTime;
                case MTGFormatsEnum.pioneer:
                    return ScrapeTimes.PioneerScrapeTime;
                case MTGFormatsEnum.pauper:
                    return ScrapeTimes.PauperScrapeTime;
                case MTGFormatsEnum.legacy:
                    return ScrapeTimes.LegacyScrapeTime;
                case MTGFormatsEnum.vintage:
                    return ScrapeTimes.VintageScrapeTime;
                default:
                    return DateTime.MinValue;
            }
        }

        private DateTime GetLastDeliveredTime(MTGFormatsEnum format, ulong guildId)
        {
            switch (format)
            {
                case MTGFormatsEnum.standard:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Standard;
                case MTGFormatsEnum.modern:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Modern;
                case MTGFormatsEnum.pioneer:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Pioneer;
                case MTGFormatsEnum.pauper:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Pauper;
                case MTGFormatsEnum.legacy:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Legacy;
                case MTGFormatsEnum.vintage:
                    return MoversShakersJSONController.ReadMoversShakersConfig(guildId).LastDeliveredTime_Vintage;
                default:
                    return DateTime.MinValue;
            }
        }

        private Models.DiscordServerChannelModel UpdateLastDeliveredTime(Models.DiscordServerChannelModel discordInformation, MTGFormatsEnum format, DateTime timeToUpdate)
        {
            switch (format)
            {
                case MTGFormatsEnum.standard:
                    discordInformation.LastDeliveredTime_Standard = timeToUpdate;
                    break;
                case MTGFormatsEnum.modern:
                    discordInformation.LastDeliveredTime_Modern = timeToUpdate;
                    break;
                case MTGFormatsEnum.pioneer:
                    discordInformation.LastDeliveredTime_Pioneer = timeToUpdate;
                    break;
                case MTGFormatsEnum.pauper:
                    discordInformation.LastDeliveredTime_Pauper = timeToUpdate;
                    break;
                case MTGFormatsEnum.legacy:
                    discordInformation.LastDeliveredTime_Legacy = timeToUpdate;
                    break;
                case MTGFormatsEnum.vintage:
                    discordInformation.LastDeliveredTime_Vintage = timeToUpdate;
                    break;
                default:
                    break;
            }
            return discordInformation;
        }

        public async void DetermineDelivery(DiscordSocketClient Client)
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Started. ###"));
            var serverInformation = MoversShakersJSONController.ReadRegisteredDiscordGuilds();

            foreach (var guild in serverInformation.ListOfRegisteredDiscordGuilds)
            {
                var currentGuildInformation = MoversShakersJSONController.ReadMoversShakersConfig(guild) ?? new Models.DiscordServerChannelModel
                {
                    LastDeliveredTime_Standard = DateTime.MinValue,
                    LastDeliveredTime_Modern = DateTime.MinValue,
                    LastDeliveredTime_Pioneer = DateTime.MinValue,
                    LastDeliveredTime_Pauper = DateTime.MinValue,
                    LastDeliveredTime_Legacy = DateTime.MinValue,
                    LastDeliveredTime_Vintage = DateTime.MinValue
                };
                foreach (var format in currentGuildInformation.ListOfFormats)
                {                    
                    Enum.TryParse(format, out MTGFormatsEnum parsedEnumValue);
                    UpdateScrapeTime(parsedEnumValue);
                    var lastScrapeTime = GetCurrentScrapeTime(parsedEnumValue);
                    var currentLastDeliveredTime = GetLastDeliveredTime(parsedEnumValue, guild);
                    if (lastScrapeTime != currentLastDeliveredTime && lastScrapeTime != DateTime.MinValue)
                    {                        
                        Console.WriteLine(ConsoleWriteOverride.AddTimeStamp($"{lastScrapeTime.ToString("hh:mm:ss")} not equal to {currentLastDeliveredTime.ToString("hh:mm:ss")}. Delivering {format.ToString()} to {guild}"));
                        UpdateScrapeTime(parsedEnumValue);
                        currentGuildInformation = UpdateLastDeliveredTime(currentGuildInformation, parsedEnumValue, lastScrapeTime);
                        MoversShakersJSONController.UpdateServerInfo(currentGuildInformation);
                        await new MTGMoversShakersOutput().DeliverMoversOutputAsync(Client, format, guild);
                    }
                    else if (lastScrapeTime == DateTime.MinValue) // This may be unnecessary as the edge case has been taken care of due to re-written logic. Keeping just in-case.
                    {
                        UpdateScrapeTime(parsedEnumValue);
                        currentGuildInformation = UpdateLastDeliveredTime(currentGuildInformation, parsedEnumValue, lastScrapeTime);
                        MoversShakersJSONController.UpdateServerInfo(currentGuildInformation);
                        await new MTGMoversShakersOutput().DeliverMoversOutputAsync(Client, format, guild);
                    }
                }                
            }
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Completed. ###"));
        }
    }
}