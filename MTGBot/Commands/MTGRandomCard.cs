using Discord.Commands;
using MTGBot.DataLookup;
using MTGBot.Embed_Output;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGRandomCard : ModuleBase<SocketCommandContext>
    {
        [Command("mtgrandom")]
        public async Task MTGRandomCards(bool isCommander = true)
        {
            MTGCardOutput Output = new MTGCardOutput();
            await Context.Channel.SendMessageAsync("", false, Output.CardOutput(GetScryFallData.PullScryFallRandomCard(isCommander)).Build());
        }
    }
}
