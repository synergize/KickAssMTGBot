using Discord;
using Discord.WebSocket;
using MoversAndShakersScrapingService.Data_Models;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers;
using MTGBot.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using MTGBot.Extensions;

namespace MTGBot.Embed_Output
{
    public class MTGMoversShakersOutput
    {
        public static DateTime MoversShakersTimeStamp = DateTime.MinValue;
        private Color successfulColor = Color.DarkGreen;
        private const int failedColor = 16580608;
        private const string footerMessage = "Contact Coaction#5994 for any bugs.";
        private EmbedBuilder GetDailyIncreaseMoversOutput(MoverCardDataModel cardsList)
        {
            var titleUrl = $"https://www.mtggoldfish.com/movers/paper/{cardsList.Format}";
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.WithUrl(titleUrl.ToLower());
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            BuildEmbed.Title = $"Daily Price Winners for {cardsList.Format}!";
            BuildEmbed.Color = successfulColor;
            BuildEmbed.WithFooter(footerMessage);
            BuildEmbed.WithTimestamp(cardsList.PageLastUpdated.ToLocalTime());
            foreach (var item in cardsList.DailyIncreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        private EmbedBuilder GetDailyDecreaseMoversOutput(MoverCardDataModel cardsList)
        {
            var titleUrl = $"https://www.mtggoldfish.com/movers/paper/{cardsList.Format}";
            EmbedBuilder BuildEmbed = new EmbedBuilder();
            BuildEmbed.WithUrl(titleUrl.ToLower());
            cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
            BuildEmbed.Title = $"Daily Price Losers for {cardsList.Format}!";
            BuildEmbed.Color = Color.LightOrange;
            BuildEmbed.WithFooter(footerMessage);
            BuildEmbed.WithTimestamp(cardsList.PageLastUpdated.ToLocalTime());
            foreach (var item in cardsList.DailyDecreaseList)
            {
                BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
            }
            return BuildEmbed;
        }

        //private EmbedBuilder GetWeeklyDecreaseMoversOutput(MoverCardDataModel cardsList)
        //{
        //    cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
        //    EmbedBuilder BuildEmbed = new EmbedBuilder();
        //    BuildEmbed.Title = $"Weekly Price Losers for {cardsList.Format}!";
        //    BuildEmbed.Color = Color.LightOrange;
        //    BuildEmbed.WithFooter(footerMessage);
        //    BuildEmbed.WithTimestamp(cardsList.PageLastUpdated);
        //    foreach (var item in cardsList.WeeklyDecreaseList)
        //    {
        //        BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
        //    }
        //    return BuildEmbed;
        //}

        //private EmbedBuilder GetWeeklyIncreaseMoversOutput(MoverCardDataModel cardsList)
        //{
        //    cardsList.Format = char.ToUpper(cardsList.Format[0]) + cardsList.Format.Substring(1);
        //    EmbedBuilder BuildEmbed = new EmbedBuilder();
        //    BuildEmbed.Title = $"Weekly Price Winners for {cardsList.Format}!";
        //    BuildEmbed.Color = successfulColor;
        //    BuildEmbed.WithFooter(footerMessage);
        //    foreach (var item in cardsList.WeeklyIncreaseList)
        //    {
        //        BuildEmbed.AddField($"__{item.Name}__", $"Change: {item.PriceChange} \nPrice: ${item.TotalPrice} \nPercentage: {item.ChangePercentage}", true);
        //    }
        //    return BuildEmbed;
        //}

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

        public EmbedBuilder NoConfiguredServerErrorOutputDeliverCards()
        {
            return new EmbedBuilder
            {
                Title = "Configuration Error",
                Description = "There was a problem finding your configured channel. I was able to deliver the prices to your default channel but please type !mtgsetchannel #<channel name> for this message to disappear or contact my developer.",
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

        public EmbedBuilder ChannelAlreadyConfiguredErrorMessage(string channelName)
        {
            return new EmbedBuilder
            {
                Title = "Channel Already Exists",
                Description = $"{channelName.CapitalizeFirstLetter()} is already configured as your destination channel. Please choose another or let me do my thing!",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = new Color(failedColor)
            };
        }

        public EmbedBuilder FormatAlreadyExistsErrorMessage(string formatName)
        {
            return new EmbedBuilder
            {
                Title = "Format Already Exists",
                Description = $"{formatName.CapitalizeFirstLetter()} is already configured. Please choose another format or skedaddle!",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = new Color(failedColor)
            };
        }

        public EmbedBuilder FormatDoesntExistErrorMessage(string formatName)
        {
            return new EmbedBuilder
            {
                Title = "Format Doesn't Exist",
                Description = $"{formatName.CapitalizeFirstLetter()} isn't configured for your server. Please double check your entry or scram!",
                Timestamp = DateTime.Now,
                Footer = new EmbedFooterBuilder() { Text = footerMessage },
                Color = new Color(failedColor)
            };
        }

        public async Task DeliverMoversOutputAsync(DiscordSocketClient Client, string format, ulong guild)
        {
            
            var guildConfig = MoversShakersJSONController.ReadMoversShakersConfig(guild);
            var scrapedData = MoversShakersJSONController.GetMoverCardScrapedData($"{format}.json");

            if (!(Client.GetGuild(guildConfig.serverID).GetChannel(guildConfig.channelID) is IMessageChannel channel))
            {
                var defaultChannel = Client.GetGuild(guildConfig.serverID).DefaultChannel;
                channel = Client.GetGuild(guildConfig.serverID).GetChannel(defaultChannel.Id) as IMessageChannel;
                await defaultChannel.SendMessageAsync("", false, NoConfiguredServerErrorOutputDeliverCards().Build());                
            }            
            
            if (scrapedData.DailyIncreaseList.Count > 0)
            {
                await channel.SendMessageAsync("", false, GetDailyIncreaseMoversOutput(scrapedData).Build());
                Thread.Sleep(3000);
            }
            if (scrapedData.DailyDecreaseList.Count > 0)
            {
                await channel.SendMessageAsync("", false, GetDailyDecreaseMoversOutput(scrapedData).Build());
                Thread.Sleep(3000);
            }
            //if (scrapedData.WeeklyIncreaseList.Count > 0)
            //{
            //    await channel.SendMessageAsync("", false, GetWeeklyIncreaseMoversOutput(scrapedData).Build());
            //    Thread.Sleep(3000);
            //}
            //if (scrapedData.WeeklyDecreaseList.Count > 0)
            //{
            //    await channel.SendMessageAsync("", false, GetWeeklyDecreaseMoversOutput(scrapedData).Build());
            //}

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

        /// <summary>
        /// Function that uses <paramref name="Client"/> to help aid in sending messages to a channel.
        /// We use 
        /// </summary>
        /// <param name="Client"></param>
        public async void DetermineDelivery(DiscordSocketClient Client)
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Started. ###"));
            var serverInformation = MoversShakersJSONController.ReadRegisteredDiscordGuilds();

            foreach (var guild in serverInformation.ListOfRegisteredDiscordGuilds)
            {
                var currentGuildInformation = MoversShakersJSONController.ReadMoversShakersConfig(guild);                   

                foreach (var format in currentGuildInformation.ListOfFormats)
                {                    
                    Enum.TryParse(format, out MTGFormatsEnum parsedEnumValue);
                    UpdateScrapeTime(parsedEnumValue);
                    var lastScrapeTime = GetCurrentScrapeTime(parsedEnumValue);
                    var currentLastDeliveredTime = GetLastDeliveredTime(parsedEnumValue, guild);
                    if (lastScrapeTime != currentLastDeliveredTime)
                    {                        
                        Console.WriteLine(ConsoleWriteOverride.AddTimeStamp($"{lastScrapeTime.ToString("hh:mm:ss")} not equal to {currentLastDeliveredTime.ToString("hh:mm:ss")}. Delivering {format.ToString()} to {guild}"));
                        currentGuildInformation = UpdateLastDeliveredTime(currentGuildInformation, parsedEnumValue, lastScrapeTime);
                        MoversShakersJSONController.UpdateServerInfo(currentGuildInformation);
                        await DeliverMoversOutputAsync(Client, format, guild);
                    }
                }                
            }
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("### Delivery Check Successfully Completed. ###"));
        }
    }
}
