using MTGBot.Data;
using MTGBot.Data.ReadWriteJSON;
using MTGBot.DataLookup.Selenium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Data_Models;

namespace MTGBot.DataLookup.MTGGoldFish
{
   public class ScrapeModernMoversShakers
    {
       private GetSeleniumDriver BuildDriver;
       private IWebDriver driver;
        public ScrapeModernMoversShakers()
        {
            try
            {
                BuildDriver = new GetSeleniumDriver();
                driver = BuildDriver.CreateDriver("https://www.mtggoldfish.com/movers/paper/modern");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<MoverCardDataModel> GetListMoversShakesTable(MoversShakersEnum movertype, string elementXPath)
        {
            try
            {
                MoverCardDataModel NewCard = new MoverCardDataModel();
                List<MoverCardDataModel> DailyList = new List<MoverCardDataModel>();
                var DailyChangeIncrease = driver.FindElements(By.XPath(elementXPath));
                int elementCounter = 0;
                int nameCounter = 0;
                string[] CardNames = DetermineCardNames(movertype);


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

                MoversShakersJSONController.WriteMoverShakersJsonByFileName(DailyList, $"{movertype.ToString()}.json");
                return DailyList;
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
                driver.Close();
                throw new Exception("Undefined exception occured. Selenium driver closed.");
            }
        }

        private string[] DetermineCardNames(MoversShakersEnum moverType)
        {
            switch (moverType)
            {
                default:
                    return null;
                case MoversShakersEnum.DailyIncrease:
                    return GetDailyIncreaseNames();
                case MoversShakersEnum.DailyDecrease:
                    return GetDailyDecreaseNames();
                case MoversShakersEnum.WeeklyIncrease:
                    return GetWeeklyIncreaseNames();
                case MoversShakersEnum.WeeklyDecrease:
                    return GetWeeklyDecreaseNames();
            }
        }
        private string[] GetDailyIncreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {

                var DailyName = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = DailyName.Text;
            }
            return NameArry;
            
        }
        private string[] GetDailyDecreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
        private string[] GetWeeklyIncreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
        private string[] GetWeeklyDecreaseNames()
        {
            string[] NameArry = new string[10];

            for (int i = 0; i < NameArry.Length; i++)
            {
                var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                NameArry[i] = Name.Text;
            }
            return NameArry;
        }
    }
}
