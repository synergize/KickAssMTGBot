﻿using MTGBot.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.DataLookup.Selenium
{
    class GetSeleniumDriver
    {
        public IWebDriver CreateDriver(string url)
        {
            var option = new ChromeOptions();
            option.AddArgument("--headless");
            option.AddArgument("--ignore-certificate-errors");
            option.AddArgument("--incognito");
            IWebDriver driver = new ChromeDriver(FilePathingStaticData.BuildFilePathDirectory(FilePathingStaticData._DriverDirectory), option);
            driver.Manage().Window.Maximize();
            try
            {                
                driver.Navigate().GoToUrl(url);
                return driver;
            }
            catch
            {
                return driver;
            }
        }
    }
}