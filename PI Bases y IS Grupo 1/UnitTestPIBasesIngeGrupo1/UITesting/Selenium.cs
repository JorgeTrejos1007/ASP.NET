using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace UnitTestPIBasesIngeGrupo1.UITesting
{
    [TestClass]
    public class Selenium
    {
        IWebDriver driver;
        [TestMethod]
        public void pruebaDeVerMaterial()
        {
            ///Arrange
            ///Crea el driver de Chrome
            driver = new ChromeDriver(Environment.CurrentDirectory);
            string URL = "https://localhost:44326/Login/LoginMiembro";
            ///Pone la pantalla en full screen
            driver.Manage().Window.Maximize();
            ///Act
            //////Se va a la URL indicada
            driver.Url = URL;
          
            IWebElement campoCorreo = driver.FindElement(By.Id("correo"));
            IWebElement campoPass = driver.FindElement(By.Id("pass"));
 
            campoCorreo.SendKeys("ronnyale0@hotmail.com");
            campoPass.SendKeys("Esquivel");
            IWebElement form = driver.FindElement(By.Id("myForm"));
            form.Submit();
            IWebElement cursos = driver.FindElement(By.Id("cursos"));
            cursos.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(ExpectedConditions.ElementToBeClickable(By.Id("cursosInscritos")));
            driver.FindElement(By.Id("cursosInscritos")).Click();
            IWebElement cursoElejido = driver.FindElement(By.Id("botonParaPerfil"));
            cursoElejido.Click();
            IWebElement seccion = driver.FindElement(By.Id("seccion"));
            seccion.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(ExpectedConditions.ElementToBeClickable(By.Id("material")));
            IWebElement material = driver.FindElement(By.Id("material"));
            var archivo = material.GetAttribute("href");
            driver.Navigate().GoToUrl(archivo);
           


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
