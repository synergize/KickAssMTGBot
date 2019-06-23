using MTGBot.Data;
using MTGBot.DataLookup.Selenium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Data_Models;

namespace MTGBot.DataLookup.MTGGoldFish
{
   public class ScrapeModernMoversShakers
    {
       private GetSeleniumDriver BuildDriver;
       private IWebDriver driver;
        public ScrapeModernMoversShakers()
        {
            BuildDriver = new GetSeleniumDriver();
            driver = BuildDriver.CreateDriver("https://www.mtggoldfish.com/movers/paper/modern");
        }
        
        public List<MoverCardDataModel> GetListDailyChangeIncrease(MoversShakersEnum movertype, string elementXPath)
        {
            MoverCardDataModel NewCard = new MoverCardDataModel();
            List<MoverCardDataModel> DailyList = new List<MoverCardDataModel>();
            var DailyChangeIncrease = driver.FindElements(By.XPath(elementXPath));
            int elementCounter = 0;
            int nameCounter = 0;
            string[] CardNames = null;

            switch (movertype)
            {
                default:
                    break;
                case MoversShakersEnum.DailyIncrease:
                    CardNames = GetDailyIncreaseNames();
                    break;
                case MoversShakersEnum.DailyDecrease:
                    CardNames = GetDailyDecreaseNames();
                    break;
                case MoversShakersEnum.WeeklyIncrease:
                    break;
                case MoversShakersEnum.WeeklyDecrease:
                    break;
            }

            foreach (var item in DailyChangeIncrease)
            {
                switch (elementCounter)
                {
                    default:

                        break;
                    case 0:
                        NewCard.PriceChange = item.Text;
                        elementCounter++;
                        break;
                    case 1:
                        NewCard.TotalPrice = item.Text;
                        elementCounter++;
                        break;
                    case 2:
                        NewCard.ChangePercentage = item.Text;
                        elementCounter = 0;
                        DailyList.Add(NewCard);
                        NewCard.Name = CardNames[nameCounter];
                        nameCounter++;
                        NewCard = new MoverCardDataModel();
                        break;
                }
                
            }

            return DailyList;

        }
        private string[] GetDailyIncreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var DailyName = driver.FindElement(By.XPath($"/html/body/div[2]/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = DailyName.Text;
            }
            return NameArry;
            
        }
        private string[] GetDailyDecreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/div[2]/div[6]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
        private string[] GetWeeklyIncreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/div[2]/div[6]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
        private string[] GetWeeklyDecreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/div[2]/div[6]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
    }
}
