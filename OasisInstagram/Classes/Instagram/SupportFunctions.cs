using OasisInstagram.Classes.Entity;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace OasisInstagram.Classes.Instagram
{
    public class SupportFunctions
    {
        public void IsPageChanged()
        {
            var changedPage = false;
            var firstAtama = true;
            var firstUrl = "";
            while (!changedPage)
            {
                if (firstAtama)
                {
                    firstUrl = Main.driver.Url;
                    firstAtama = false;
                }
                if (firstUrl != "" && firstUrl != Main.driver.Url)
                {
                    changedPage = true;
                }
            }
            IsPageLoaded();//Sayfa yüklenene kadar bekler
        }
        public void IsPageLoaded()
        {
            var isPageLoaded = false;
            while (!isPageLoaded)
            {
                isPageLoaded = Convert.ToBoolean(((IJavaScriptExecutor)Main.driver).ExecuteScript("return document.readyState").Equals("complete"));
                if (isPageLoaded)
                {
                    Console.WriteLine("sayfa yüklendi........");
                    new OasisInstagram.Data.Transfer().ToFile("sayfa yüklendi........");
                }
            }            
        }
        public void IsLoginScreenLoaded()
        {
            var isPageLoaded = false;
            while (!isPageLoaded)
            {
                try
                {
                    IWebElement usernameBoxVar = Main.driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[1]/div/label/input"));
                    if (usernameBoxVar != null)
                    {
                        isPageLoaded = true;
                    }
                    if (isPageLoaded)
                    {
                        Console.WriteLine("sayfa yüklendi........");
                        new OasisInstagram.Data.Transfer().ToFile("sayfa yüklendi........");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("hata = " + e.Message);
                    new OasisInstagram.Data.Transfer().ToFile("hata = " + e.Message);

                }
            }
        }

        public void IsFollowUserPageLoaded()
        {
            var isPageLoaded = false;
            while (!isPageLoaded)
            {
                try
                {
                    bool isUserNotFind = Main.driver.FindElements(By.XPath("//*[text()='Üzgünüz, bu sayfaya ulaşılamıyor.']")).Count > 0;
                    bool usernameBoxVar = false;
                    if (!isUserNotFind)
                    {
                        usernameBoxVar = Main.driver.FindElements(By.XPath("//*[text()='Takip Et']")).Count > 0;
                        if (!usernameBoxVar)
                        {
                            usernameBoxVar = Main.driver.FindElements(By.XPath("//*[@aria-label='Takiptesin']")).Count > 0;
                            if (!usernameBoxVar)
                            {
                                usernameBoxVar = Main.driver.FindElements(By.XPath("//*[text() = 'İstek Gönderildi']")).Count > 0;
                            }
                        }
                    }
                    
                    if (usernameBoxVar || isUserNotFind)
                    {
                        isPageLoaded = true;
                    }
                    if (isPageLoaded)
                    {
                        Console.WriteLine("sayfa yüklendi........");
                        new OasisInstagram.Data.Transfer().ToFile("Takipçi sayfası yüklendi........");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("hata = " + e.Message);

                }
            }
        }
        public List<User> List2UserList(List<string> list)
        {
            
            List<User> users = new List<User>();
            foreach (var item in list)
            {
                User user = new User();
                user._userName = item;
                user._followDate = DateTime.Now.ToShortDateString();
                users?.Add(user);
            }
            return users;
        }
    }
}
