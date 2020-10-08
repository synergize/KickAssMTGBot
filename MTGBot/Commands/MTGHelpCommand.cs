using Discord.Commands;
using MTGBot.Embed_Output;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGHelpCommand : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task MTGHelp()
        {
            MTGCardOutput Help = new MTGCardOutput();
            await Context.Channel.SendMessageAsync("", false, Help.GettingHelp().Build());
        }
    }
}
