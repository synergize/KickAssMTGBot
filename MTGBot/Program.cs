using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.DataLookup;
using MTGBot.Embed_Output;
using MTGBot.Helpers;
using MTGBot.User_Message_Handler;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MTGBot
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp("Loading Filters."));
            LegalityDictionary.LoadLegalityDict();

            var aTimer = new System.Timers.Timer(1800000); //thirty minutes in milliseconds
            aTimer.BeginInit();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.EndInit();
            aTimer.Start();


            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug

            });

            Client.MessageReceived += Client_MessageRecieved;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.Ready += Client_Ready;
            Client.Log += Log;

            string Token = BotToken.GetTokenString();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            Thread.Sleep(10000); //10 seconds, sleeps a differrent thread from the bot so that it can finish loading before we begin.
            new MTGMoversShakersOutput().DetermineDelivery(Client);
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(ConsoleWriteOverride.AddTimeStamp(msg.Message));
            return Task.CompletedTask;
        }

        private async Task Client_MessageRecieved(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);
            MTGCardOutput GetCard = new MTGCardOutput();

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;
            if (Context.User.Id == 129804455964049408 && Context.Guild.Id == 596104949503361050)
            {
                await ReactWithEmoteAsync(Context.Message, "<:WeebsOut:627783662708064256>");
            }
            if (Message.Content.Contains("[") && Message.Content.Contains("]"))
            {
                try
                {
                    Regex rx = new Regex(@"\[\[(.*?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection matches = rx.Matches(Message.Content);
                    foreach (var item in matches)
                    {
                        var dataOutput = new UserMessageController(item.ToString());
                        await Context.Channel.SendMessageAsync("", false, dataOutput.MessageOutput.Build());
                    }
                }
                catch(Exception msg)
                {
                    Console.WriteLine(ConsoleWriteOverride.AddTimeStamp(msg.Message));
                    await Context.Channel.SendMessageAsync("", false, GetCard.DetermineFailure(3).Build());
                }
            }

            int ArgPos = 0;
            if (!(Message.HasCharPrefix('!', ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;

            var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
            if (!Result.IsSuccess)
                Console.WriteLine(ConsoleWriteOverride.AddTimeStamp($"Something went wrong with executing command. Text: {Context.Message.Content} | Error: {Result.ErrorReason}"));
        }
        private async Task Client_Ready()
        {
            await Client.SetGameAsync("Magic: The Gathering !mtghelp", null, ActivityType.Playing);
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
