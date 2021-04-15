using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTGBot.Embed_Output;
using MTGBot.Helpers;
using MTGBot.User_Message_Handler;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DiscordNetHelperLibrary;
using log4net;
using log4net.Config;
using MTGBot.DataLookup;
using Timer = System.Timers.Timer;

namespace MTGBot
{
    internal class Program : DiscordBotBase
    {
        private static string Token => Environment.GetEnvironmentVariable("MTG_BOT");
        private static void Main(string[] args) => new Program().MtgMainAsync().GetAwaiter().GetResult();

        private async Task MtgMainAsync()
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("Loading Filters."));
            LegalityDictionary.LoadLegalityDict();

            //var aTimer = new Timer(1800000); //thirty minutes in milliseconds
            //aTimer.BeginInit();
            //aTimer.Elapsed += OnTimedEvent;
            //aTimer.EndInit();
            //aTimer.Start();
            var logRepository = LogManager.GetRepository(typeof(Program).Assembly);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            await MainAsync(Token);
            await Commands.AddModulesAsync(typeof(Program).Assembly, null);
            Client.MessageReceived += Client_MessageRecieved;
            Client.Ready += Client_Ready;

            //Thread.Sleep(10000); //10 seconds, sleeps a different thread from the bot so that it can finish loading before we begin.
            //new MTGMoversShakersOutput().DetermineDelivery(Client);
            await Task.Delay(-1);
        }

        private async Task Client_MessageRecieved(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            var context = new SocketCommandContext(Client, message);
            var getCard = new MTGCardOutput();

            if (context.Message == null || context.Message.Content == "") return;
            if (context.User.IsBot) return;
            if (context.User.Id == 129804455964049408 && context.Guild.Id == 596104949503361050)
            {
                await ReactWithEmoteAsync(context.Message, "<:WeebsOut:627783662708064256>");
            }
            if (message != null && (message.Content.Contains("[") && message.Content.Contains("]")))
            {
                try
                {
                    Regex rx = new Regex(@"\[\[(.*?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection matches = rx.Matches(message.Content);
                    foreach (var item in matches)
                    {
                        var dataOutput = new UserMessageController(item.ToString());
                        await context.Channel.SendMessageAsync("", false, dataOutput.MessageOutput.Build());
                    }
                }
                catch(Exception msg)
                {
                    Console.WriteLine(ConsoleWriteOverride.AddTimeStamp(msg.Message));
                    await context.Channel.SendMessageAsync("", false, getCard.DetermineFailure(3).Build());
                }
            }

            var argPos = 0;
            if (!(message.HasStringPrefix("mtg!", ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos))) return;

            var result = await Commands.ExecuteAsync(context, argPos, null);
            if (!result.IsSuccess)
                Console.WriteLine(ConsoleWriteOverride.AddTimeStamp($"Something went wrong with executing command. Text: {context.Message.Content} | Error: {result.ErrorReason}"));
        }
        private async Task Client_Ready()
        {
            await Client.SetGameAsync("mtg!help to get started!", null, ActivityType.Playing);
        }

        //If someone adds a reaction, run x code. 
        private void OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel Channel, SocketReaction Reaction)
        {
            //If a bot sends the reaction, disregard. 
            if (((SocketUser)Reaction.User).IsBot) return;
        }
        private async Task ReactWithEmoteAsync(SocketUserMessage userMsg, string escapedEmote)
        {
            
            if (Emote.TryParse(escapedEmote, out var emote))
            {
                await userMsg.AddReactionAsync(emote);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {            
            new MTGMoversShakersOutput().DetermineDelivery(Client);
        }        
    }
}
