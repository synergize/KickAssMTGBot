using Discord.Commands;
using MTGBot.DataLookup.MTGGoldFish;
using MTGBot.Embed_Output;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
