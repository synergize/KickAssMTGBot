using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTGBot.Data;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Helpers.Enums;
using System;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGMoversShakersCommand : ModuleBase<SocketCommandContext>
    {        
        [Command("mtgmovers")]
        public async Task MTGMovers()
        {
            Console.WriteLine("Before New Instance.");           
            Console.WriteLine("After Instance");
            //await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyIncreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.DailyIncrease, MoversShakersClassXPathingStatic.DailyIncreaseXpath)).Build());
            //await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyDecreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.DailyDecrease, MoversShakersClassXPathingStatic.DailyDecreaseXpath)).Build());
            //await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetWeeklyIncreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.WeeklyIncrease, MoversShakersClassXPathingStatic.WeeklyIncreaseXpath)).Build());
            //await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetWeeklyDecreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.WeeklyDecrease, MoversShakersClassXPathingStatic.WeeklyDecreaseXpath)).Build());
            var test = Context.Channel;
            Console.WriteLine("After Message");
        }

        [Command("mtgsetchannel")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task SetChannelConfiguration(ISocketMessageChannel channel)
        {
            var discordServerId = Context.Guild.Id;
            var discordServerInformation = MoversShakersJSONController.ReadMoversShakersConfig(discordServerId);            

            if (discordServerInformation == null)
            {
                MoversShakersJSONController.UpdateServerInfo(new Models.DiscordServerChannelModel
                {
                  serverID = discordServerId,
                  channelID = channel.Id
                });
                // Send message - New document created. 
            }
            else
            {
                discordServerInformation.channelID = channel.Id;
                MoversShakersJSONController.UpdateServerInfo(discordServerInformation);
                // Send message - Doucment Updated.
            }
        }

        [Command("mtgaddformat")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task AddFormatForScrape(string formatName)
        {
            var userInput = formatName.ToLower();
            var discordServerId = Context.Guild.Id;

            if (Enum.IsDefined(typeof(MTGFormatsEnum), userInput))
            {
                var serverInformation = MoversShakersJSONController.ReadMoversShakersConfig(discordServerId);

                if (serverInformation == null)
                {                    
                    // Send message to user to have them set a channel.
                }
                else
                {
                    MoversShakersJSONController.AddChannelFormat(serverInformation, userInput);
                }                
            }
            else
            {
                // Let user know to check their input, not a valid format. 
            }
        }

        [Command("mtgremoveformat")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task RemoveFormatForScrape(string formatName)
        {
            var userInput = formatName.ToLower();
            var discordServerId = Context.Guild.Id;

            if (Enum.IsDefined(typeof(MTGFormatsEnum), userInput))
            {
                var serverInformation = MoversShakersJSONController.ReadMoversShakersConfig(discordServerId);

                if (serverInformation == null)
                {
                    // Send message to user to have them set a channel.
                }
                else
                {
                    MoversShakersJSONController.RemoveChannelFormat(serverInformation, userInput);
                }
            }
            else
            {
                // Let user know to check their input, not a valid format. 
            }
        }

    }
}
