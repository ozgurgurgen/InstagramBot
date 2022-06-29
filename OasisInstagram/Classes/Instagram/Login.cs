using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OasisInstagram.Classes.Instagram
{
    public class Login
    {
        
        public void Start()
        {
            Main.driver.Navigate().GoToUrl("https://instagram.com");
            new SupportFunctions().IsLoginScreenLoaded(); //Giriş ekranı yüklenene kadar bekler

            IWebElement usernameBox = Main.driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[1]/div/label/input"));
            IWebElement passwordBox = Main.driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[2]/div/label/input"));
            IWebElement instagramLogin = Main.driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[3]/button"));

           // Thread.Sleep(InstagramSettings.Default.asama10);//Aşama 10
            usernameBox.SendKeys(InstagramSettings.Default.instagramUserName);
            passwordBox.SendKeys(InstagramSettings.Default.instagramPassword);
            instagramLogin.Click();

        }
    }
}
