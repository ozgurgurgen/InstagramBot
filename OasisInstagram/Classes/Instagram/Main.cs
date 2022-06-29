using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace OasisInstagram.Classes.Instagram
{
    public class Main
    {
        public static WebDriver driver;
        public void Start(string username)
        {           
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            new Login().Start(); //Instagram login process
            new SupportFunctions().IsPageChanged(); // waits until the page is changed
            new GetFollowAndFollowers().Get(); 
            new Data.Json().WillUnfollow();
        }

        public void Deneme(string username)
        {
            new Data.Json().GetFollowersByUserName(username, driver);
        }
        
        
    }
}
