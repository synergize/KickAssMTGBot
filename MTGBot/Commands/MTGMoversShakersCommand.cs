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
        [Command("setchannel", RunMode = RunMode.Sync)]
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
            else if (!discordServerInformation.channelID.Equals(channel.Id))
            {
                // Channel wasn't set.
                discordServerInformation.channelID = channel.Id;
                MoversShakersJSONController.UpdateServerInfo(discordServerInformation);
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().SetChannelSuccess(channel).Build());
            }
            else
            {
                // Channel was already set.
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().ChannelAlreadyConfiguredErrorMessage(channel.Name).Build());
            }
        }

        [Command("addformat", RunMode = RunMode.Sync)]
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
                else if (!serverInformation.ListOfFormats.Contains(userInput))
                {
                    // User didn't already have the format set.
                    MoversShakersJSONController.AddChannelFormat(serverInformation, userInput);
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().AddFormatSuccess(formatName).Build());
                }
                else
                {
                    // User already added format.
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().FormatAlreadyExistsErrorMessage(formatName).Build());
                }
            }
            else
            {
                // Let user know to check their input, not a valid format. 
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().IncorrectOrUnspportedFormatError().Build());
            }
        }

        [Command("removeformat", RunMode = RunMode.Sync)]
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
                    return;
                }
                else if (serverInformation.ListOfFormats.Contains(userInput))
                {
                    // User did have format in the list so we're removing.
                    MoversShakersJSONController.RemoveChannelFormat(serverInformation, userInput);
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().RemoveFormatSuccess(formatName).Build());
                    return;
                }
                else
                {
                    // User already removed channel.
                    await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().FormatDoesntExistErrorMessage(formatName).Build());
                }
            }
            else
            {
                // Let user know to check their input, not a valid format. 
                await Context.Channel.SendMessageAsync("", false, new MTGMoversShakersOutput().IncorrectOrUnspportedFormatError().Build());
            }
        }

        [Command("formatadd", RunMode = RunMode.Sync)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task FormatAddForScrape(string formatName)
        {
            await AddFormatForScrape(formatName);
        }

        [Command("formatremove", RunMode = RunMode.Sync)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task FormatRemoveForScrape(string formatName)
        {
            await RemoveFormatForScrape(formatName);
        }

        [Command("formatdelete", RunMode = RunMode.Sync)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task FormatDeleteForScrape(string formatName)
        {
            await RemoveFormatForScrape(formatName);
        }

        [Command("deleteformat", RunMode = RunMode.Sync)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task DeleteFormatForScrape(string formatName)
        {
            await RemoveFormatForScrape(formatName);
        }



    }
}
