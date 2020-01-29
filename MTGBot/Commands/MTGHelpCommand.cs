using Discord.Commands;
using MTGBot.Embed_Output;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGHelpCommand : ModuleBase<SocketCommandContext>
    {
        [Command("mtghelp")]
        public async Task MTGHelp()
        {
            MTGCardOutput Help = new MTGCardOutput();
            await Context.Channel.SendMessageAsync("", false, Help.GettingHelp().Build());
        }

        [Command("helpmtg")]
        public async Task HelpMTG()
        {
            await MTGHelp();
        }

    }
}
