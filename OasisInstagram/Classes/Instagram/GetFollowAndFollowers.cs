using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Threading;

namespace OasisInstagram.Classes.Instagram
{

    public class GetFollowAndFollowers
    {

        public void Get()
        {
            string getFollows = null;
            string getFollowers = null;
            Thread.Sleep(500);
            Main.driver.Navigate().GoToUrl($"https://www.instagram.com/{InstagramSettings.Default.instagramUserName}/following/");
            //Main.driver.Navigate().GoToUrl("https://instagram.com/" + InstagramSettings.Default.instagramUserName + "/followers/");
            new SupportFunctions().IsPageLoaded(); // sayfa yüklenene kadar bekler

            ((IJavaScriptExecutor)Main.driver).ExecuteScript(Properties.Resources.MyFollows);

            IJavaScriptExecutor followingJS = (IJavaScriptExecutor)Main.driver;


            bool isLoaded = false;
            while (!isLoaded)
            {
                try
                {
                    getFollows = Convert.ToString(followingJS.ExecuteScript("return document.querySelector('#takipler').innerText"));
                    if (getFollows != null)
                    {
                        isLoaded = true;
                    }
                }
                catch (Exception)
                {

                }
            }
            Thread.Sleep(500);
            Main.driver.Navigate().GoToUrl($"https://www.instagram.com/{InstagramSettings.Default.instagramUserName}/followers/");
            //Main.driver.Navigate().GoToUrl("https://instagram.com/" + InstagramSettings.Default.instagramUserName + "/following/");

            new SupportFunctions().IsPageLoaded(); // sayfa yüklenene kadar bekler

            ((IJavaScriptExecutor)Main.driver).ExecuteScript(Properties.Resources.MyFollowers);
            IJavaScriptExecutor followerJS = (IJavaScriptExecutor)Main.driver;


            bool isLoaded2 = false;
            while (!isLoaded2)
            {
                try
                {
                    getFollowers = Convert.ToString(followerJS.ExecuteScript("return document.querySelector('#takipciler').innerText"));
                    if (getFollowers != null)
                    {
                        isLoaded2 = true;
                    }
                }
                catch (Exception)
                {
                }
            }


            List<string> followerList = new List<string>();
            List<string> followingList = new List<string>();
            List<string> unfollowers = new List<string>();

            followerList.AddRange(getFollowers.Split(','));
            followingList.AddRange(getFollows.Split(','));
            followingList.Remove("");
            followerList.Remove("");
            Console.WriteLine(followerList.Count);
            foreach (string follower in followerList)
            {
                Console.WriteLine("Takipçi => " + follower);
            }
            Console.WriteLine("ÇALIŞTI VE BİTTİ");
            Entity.DataPatern dataFollower = new Entity.DataPatern();
            dataFollower._tableName = "Followers";
            dataFollower._usersList = new SupportFunctions().List2UserList(followerList);
            new Data.Json().Write(dataFollower, "FollowersDB");
            Entity.DataPatern dataFollows = new Entity.DataPatern();
            dataFollows._tableName = "Following";
            dataFollows._usersList = new SupportFunctions().List2UserList(followingList);
            new Data.Json().Write(dataFollows, "FollowingDB");
            new Data.Json().Write(dataFollows, "FollowingDB");
            new Data.Json().WillUnfollow();

        }



    }
}
