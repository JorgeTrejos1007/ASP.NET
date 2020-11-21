using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace UnitTestPIBasesIngeGrupo1.UITesting
{
    [TestClass]
    public class Selenium
    {
        IWebDriver driver;
        [TestMethod]
        public void TestMethod1()
        {
            ///Arrange
            ///Crea el driver de Chrome
            driver = new ChromeDriver(Environment.CurrentDirectory);
            string URL = "https://localhost:44326/";
            ///Pone la pantalla en full screen
            driver.Manage().Window.Maximize();
            ///Act
            //////Se va a la URL indicada
            driver.Url = URL;
            ///Assert

        }

        [TestCleanup]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}
