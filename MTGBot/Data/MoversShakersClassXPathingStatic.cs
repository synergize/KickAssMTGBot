using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.Data
{
   public static class MoversShakersClassXPathingStatic
    {
        private readonly static string DailyIncrease = "//div[contains(@class, 'movers-daily')]//div//table//tbody//tr//span[contains(@class, 'increase')]";
        private readonly static string DailyDecrease = "//div[contains(@class, 'movers-daily')]//div//table//tbody//tr//span[contains(@class, 'decrease')]";
        private readonly static string WeeklyIncrease = "//div[contains(@class, 'movers-weekly')]//div//table//tbody//tr//span[contains(@class, 'increase')]";
        private readonly static string WeeklyDecrease = "//div[contains(@class, 'movers-weekly')]//div//table//tbody//tr//span[contains(@class, 'decrease')]";

        public static string DailyIncreaseXpath
        {
            get { return DailyIncrease; }
        }
        public static string DailyDecreaseXpath
        {
            get { return DailyDecrease; }
        }
        public static string WeeklyIncreaseXpath
        {
            get { return WeeklyIncrease; }
        }
        public static string WeeklyDecreaseXpath
        {
            get { return WeeklyDecrease; }
        }
    }
}
