using Discord.Commands;
using MTGBot.Data;
using MTGBot.DataLookup.MTGGoldFish;
using MTGBot.Embed_Output;
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
            ScrapeModernMoversShakers Daily = new ScrapeModernMoversShakers();
            Console.WriteLine("After Instance");
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyIncreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.DailyIncrease, MoversShakersClassXPathingStatic.DailyIncreaseXpath)).Build());
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetDailyDecreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.DailyDecrease, MoversShakersClassXPathingStatic.DailyDecreaseXpath)).Build());
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetWeeklyIncreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.WeeklyIncrease, MoversShakersClassXPathingStatic.WeeklyIncreaseXpath)).Build());
            await Context.Channel.SendMessageAsync(null, false, MTGMoversShakersOutput.GetWeeklyDecreaseMoversOutput(Daily.GetListMoversShakesTable(MoversShakersEnum.WeeklyDecrease, MoversShakersClassXPathingStatic.WeeklyDecreaseXpath)).Build());

            Console.WriteLine("After Message");
        }
    }
}
