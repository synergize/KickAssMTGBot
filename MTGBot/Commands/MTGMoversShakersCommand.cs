using Discord.Commands;
using MTGBot.Data;
using MTGBot.DataLookup.MTGGoldFish;
using MTGBot.Embed_Output;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTGBot.Commands
{
    public class MTGMoversShakersCommand : ModuleBase<SocketCommandContext>
    {
        [Command("mtgmovers")]
        public async Task MTGMovers()
        {
            ScrapeModernMoversShakers Daily = new ScrapeModernMoversShakers();
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyIncreaseMoversOutput(Daily.GetListDailyChangeIncrease(MoversShakersEnum.DailyIncrease, MoversShakersClassXPathingStatic.DailyIncreaseXpath)).Build());
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyDecreaseMoversOutput(Daily.GetListDailyChangeIncrease(MoversShakersEnum.DailyDecrease, MoversShakersClassXPathingStatic.DailyDecreaseXpath)).Build());


        }
    }
}
