using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.Embed_Output;
using MTGBot.Helpers.Enums;
using System;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGMoversShakersCommand : ModuleBase<SocketCommandContext>
    {        
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
                    channelID = channel.Id,
                });
                // Send message - New document created. 
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().SetChannelSuccess(channel).Build());
            }
            else
            {
                discordServerInformation.channelID = channel.Id;
                MoversShakersJSONController.UpdateServerInfo(discordServerInformation);
                // Send message - Doucment Updated.
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().SetChannelSuccess(channel).Build());
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
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().NoConfiguredServerErrorOutput().Build());
                }
                else
                {
                    MoversShakersJSONController.AddChannelFormat(serverInformation, userInput);
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().AddFormatSuccess(formatName).Build());
                }                
            }
            else
            {
                // Let user know to check their input, not a valid format. 
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().IncorrectOrUnspportedFormatError().Build());
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
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().NoConfiguredServerErrorOutput().Build());
                }
                else
                {
                    MoversShakersJSONController.RemoveChannelFormat(serverInformation, userInput);
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().RemoveFormatSuccess(formatName).Build());
                }
            }
            else
            {
                // Let user know to check their input, not a valid format. 
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().IncorrectOrUnspportedFormatError().Build());
            }
        }

    }
}
