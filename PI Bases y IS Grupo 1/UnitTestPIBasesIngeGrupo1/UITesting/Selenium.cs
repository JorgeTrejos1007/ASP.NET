using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
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
        [TestMethod]
        public void pruebaDeNumeroDeIdiomasEnLaComunidad()
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

            campoCorreo.SendKeys("stevegc112016@gmail.com");
            campoPass.SendKeys("123456");
            IWebElement form = driver.FindElement(By.Id("myForm"));
            form.Submit();
            IWebElement cursos = driver.FindElement(By.Id("analitica"));
            cursos.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(ExpectedConditions.ElementToBeClickable(By.Id("analiticaComunidad")));
            driver.FindElement(By.Id("analiticaComunidad")).Click();
            IWebElement idiomas = driver.FindElement(By.Id("totalIdiomas"));
            Assert.AreEqual("12", idiomas.Text);
        }

        [TestMethod]
        public void pruebaVerificarCertificadoDisponibleParaAprobar()
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

            campoCorreo.SendKeys("stevegc112016@gmail.com");
            campoPass.SendKeys("123456");
            IWebElement form = driver.FindElement(By.Id("myForm"));
            form.Submit();
            IWebElement certificados = driver.FindElement(By.Id("certificados"));
            certificados.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(ExpectedConditions.ElementToBeClickable(By.Id("aprobar")));
            driver.FindElement(By.Id("aprobar")).Click();
            IWebElement estudiante = driver.FindElement(By.Id("estudiante"));
            Assert.AreEqual("Juan Pablo Gutierrez", estudiante.Text);
            IWebElement curso = driver.FindElement(By.Id("curso"));
            Assert.AreNotEqual("Algebra Lineal 1", curso.Text);
            IWebElement educador = driver.FindElement(By.Id("educador"));
            Assert.AreEqual("Ronny Castro Esquivel", educador.Text);
         }

        [TestMethod]
        public void pruebaVerificarCorrectitudDeMiPerfil()
        {
            driver = new ChromeDriver(Environment.CurrentDirectory);
            string URL = "https://localhost:44326/Login/LoginMiembro";
            ///Pone la pantalla en full screen
            driver.Manage().Window.Maximize();
            ///Act
            //////Se va a la URL indicada
            driver.Url = URL;

            IWebElement campoCorreo = driver.FindElement(By.Id("correo"));
            IWebElement campoPass = driver.FindElement(By.Id("pass"));

            campoCorreo.SendKeys("hellenfdz12@gmail.com");
            campoPass.SendKeys("Hellencita");
            IWebElement form = driver.FindElement(By.Id("myForm"));
            form.Submit();
            IWebElement perfil = driver.FindElement(By.Id("mi-perfil"));
            perfil.Click();
            IWebElement perfilHellen = driver.FindElement(By.Id("perfilHellen"));
            Assert.AreEqual("Hellen Fernandez Jimenez", perfilHellen.Text);

        }

        [TestMethod]
        public void pruebaVerificarCursosAprobados()
        {
            driver = new ChromeDriver(Environment.CurrentDirectory);
            string URL = "https://localhost:44326/Login/LoginMiembro";
            ///Pone la pantalla en full screen
            driver.Manage().Window.Maximize();
            ///Act
            //////Se va a la URL indicadaC:\Users\ecci\source\repos\proyecto-integrador-de-ingenieria-de-software-y-bases-de-datos-grupo-14\PI Bases y IS Grupo 1\PI Bases y IS Grupo 1\Pages\Login\LoginEstudiante.cshtml.cs
            driver.Url = URL;

            IWebElement campoCorreo = driver.FindElement(By.Id("correo"));
            IWebElement campoPass = driver.FindElement(By.Id("pass"));

            campoCorreo.SendKeys("rojas3099@gmail.com");
            campoPass.SendKeys("adriancito");
            IWebElement form = driver.FindElement(By.Id("myForm"));
            form.Submit();
            IWebElement perfil = driver.FindElement(By.Id("mi-perfil"));
            perfil.Click();
            IWebElement perfilHellen = driver.FindElement(By.Id("cursosAprobadosMiPerfil"));
            Assert.AreEqual("Introduccion a las bases de datos v.3", perfilHellen.Text);

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
