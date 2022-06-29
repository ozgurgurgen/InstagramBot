using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OasisInstagram.Classes.Entity;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OasisInstagram.Classes.Data
{
    public class Json
    {
        public static int kullanıcıBulunamadı ;
        public static int kullanıcıBulundu ;
        private static bool AutoUnfollowFirstRun = true;
        public static bool autoFollowRuning;
        public static bool autoUnfollowRuning;
        string folderPath = Environment.CurrentDirectory + $@"/Users/{InstagramSettings.Default.instagramUserName}";
        public void Write(DataPatern data, string dbName)
        {
            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string path = $@"{folderPath}/{dbName}.ozgur";

            if (!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                File.WriteAllText(path, json);
            }
            else
            {
                var jsonString = File.ReadAllText(path);
                var jsonDatas = JsonConvert.DeserializeObject<DataPatern>(jsonString);
                Console.WriteLine("data bu = " + jsonDatas._tableName);
                new OasisInstagram.Data.Transfer().ToFile("data bu = " + jsonDatas._tableName);
                DataPatern findDataPatern = new DataPatern();
                findDataPatern = jsonDatas;
                List<User> unfollower = new List<User>();

                //jsonDatas = data;
                try
                {
                    foreach (var item in findDataPatern._usersList)
                    {
                        var sorgu = data._usersList.Where(x => x._userName == item._userName).Count();
                        if (sorgu < 1)
                        {
                            //findDataPatern._usersList.Remove(item);
                            unfollower?.Add(item);
                            Console.WriteLine("Takipten Çıkan Kullanıcı: " + item._userName);
                            new OasisInstagram.Data.Transfer().ToFile("Takipten Çıkan Kullanıcı: " + item._userName);
                            //takipten çıkanları bulduk
                        }
                    }

                    foreach (var item in data._usersList)
                    {
                        var sorgu = findDataPatern._usersList.Where(x => x._userName == item._userName).Count();
                        if (sorgu < 1)
                        {
                            findDataPatern._usersList.Add(item);
                            Console.WriteLine("yeni takipçi : " + item._userName);
                            new OasisInstagram.Data.Transfer().ToFile("yeni takipçi : " + item._userName);
                            RemoveFollowRequestFile(item._userName);
                            //yeni takipçileri bulduk
                        }
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("Sorun: " + e.Message.ToString());
                    new OasisInstagram.Data.Transfer().ToFile("Sorun: " + e.Message.ToString());
                }

                for (int i = 0; i < unfollower.Count; i++)
                {
                    findDataPatern._usersList.Remove(unfollower[i]);
                }
                var jsonToString = JsonConvert.SerializeObject(findDataPatern);
                File.WriteAllText(path, jsonToString);
                var lastWriteTime = File.GetLastWriteTime(path);
                unfollower.Clear();
                /////////////////
            }
        }
        public void WillUnfollow()
        {
            List<User> _noUnfollow = new List<User>();
            List<User> _willUnfollow = new List<User>();
            string unFollowPath = $@"{folderPath}/WillUnfollow.ozgur";
            string followerPath = $@"{folderPath}/FollowersDB.ozgur";
            string followPath = $@"{folderPath}/FollowingDB.ozgur";
            if (File.Exists(followPath) && File.Exists(followerPath))
            {
                var followJsonString = File.ReadAllText(followPath);
                var followerJsonString = File.ReadAllText(followerPath);

                var followJsonDatas = JsonConvert.DeserializeObject<DataPatern>(followJsonString);
                var followerJsonDatas = JsonConvert.DeserializeObject<DataPatern>(followerJsonString);

                try
                {
                    foreach (var item in followJsonDatas._usersList)
                    {
                        var sorgu = followerJsonDatas._usersList.Where(
                            x => x._userName == item._userName
                            ).Count();
                        if (sorgu > 0)
                        {
                            _noUnfollow?.Add(item);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hata mesajı : " + e.Message.ToString());
                    new OasisInstagram.Data.Transfer().ToFile("Hata mesajı : " + e.ToString());
                }

                for (int i = 0; i < _noUnfollow.Count; i++)
                {
                    followJsonDatas._usersList.Remove(_noUnfollow[i]);

                }

                foreach (var item in followJsonDatas._usersList)
                {
                    DateTime followDate = Convert.ToDateTime(item._followDate);
                    DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    TimeSpan days = now - followDate;
                    if (days.Days >= 3)
                    {
                        Console.WriteLine("Zamanı Geçmişler => " + item._userName);
                        new OasisInstagram.Data.Transfer().ToFile("Zamanı Geçmişler => " + item._userName);
                        _willUnfollow?.Add(item);
                    }
                }
                DataPatern dataPatern = new DataPatern();
                dataPatern._tableName = "WillUnfollows";
                dataPatern._usersList = _willUnfollow;
                var jsonDatasToString = JsonConvert.SerializeObject(dataPatern);
                File.WriteAllText(unFollowPath, jsonDatasToString);

                _willUnfollow = new List<User> { null };
                _noUnfollow = new List<User> { null };
            }

        }
        public void UnfollowByUserName(string userName, IWebDriver driver)
        {
            bool processFinished = false;
            bool buttonClicked = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("isHasButton = document.querySelector('#nextUserUnfollow') != null;if(isHasButton){document.querySelector('#nextUserUnfollow').click(); return true;};"));
            if (!buttonClicked)
            {
                driver.Navigate().GoToUrl("https://instagram.com/" + userName);
            }
            new Instagram.SupportFunctions().IsPageLoaded();
            Thread.Sleep(1000);
            IJavaScriptExecutor isUserFindJS = (IJavaScriptExecutor)driver;
            bool isUserNotFind = driver.FindElements(By.XPath("//*[text()='Üzgünüz, bu sayfaya ulaşılamıyor.']")).Count > 0;

            if (isUserNotFind)
            {
                kullanıcıBulunamadı += 1;
                Console.WriteLine("Bulunamayan Kullanıcıadı => " + userName);
                new OasisInstagram.Data.Transfer().ToFile("Bulunamayan Kullanıcıadı => " + userName);
                Thread.Sleep(180000);
            }
            else
            {
                kullanıcıBulundu += 1;
            }

            if (!isUserNotFind)
            {
                if (isUserNotFind)
                {
                    processFinished = true;
                }
                IJavaScriptExecutor followUserJS = (IJavaScriptExecutor)driver;
                followUserJS.ExecuteScript(Properties.Resources.Unfollow);
                string unfollowedPath = folderPath + @"/Unfollowed.ozgur";                                

                User unfollowedUser;
                string json;

                unfollowedUser = new User { _userName = userName, _followDate = DateTime.Now.ToShortDateString() };

                if (!File.Exists(unfollowedPath))
                {
                    DataPatern dataPatern = new DataPatern();
                    dataPatern._tableName = DateTime.Now.ToShortDateString();
                    dataPatern._usersList = new List<User> { unfollowedUser };
                    json = JsonConvert.SerializeObject(dataPatern);
                }
                else
                {
                    string unfollowedFileString = File.ReadAllText(unfollowedPath);
                    var unfollowedUserDatas = JsonConvert.DeserializeObject<DataPatern>(unfollowedFileString);
                    unfollowedUserDatas._usersList?.Add(unfollowedUser);
                    json = JsonConvert.SerializeObject(unfollowedUserDatas);
                }

                while (!processFinished)
                {
                    processFinished = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return document.body.innerHTML.includes('\">Takip Et</div>')"));
                }
                File.WriteAllText(unfollowedPath, json);
            }
            //Delete user from FollowingDB.ozgur file
            string followingPath = folderPath + @"/FollowingDB.ozgur";
            string followingFileString = File.ReadAllText(followingPath);
            var followingJsonData = JsonConvert.DeserializeObject<DataPatern>(followingFileString);
            var user = followingJsonData._usersList.Where(x => x._userName == userName).FirstOrDefault();
            followingJsonData._usersList.Remove(user);
            var jsonData2String = JsonConvert.SerializeObject(followingJsonData);
            File.WriteAllText(followingPath, jsonData2String);
            //Delete user from FollowingDB.ozgur file. End
        }
        public void FollowByUserName(string userName, IWebDriver driver)
        {
            string followPath = folderPath + @"/FollowingDb.ozgur";
            bool buttonClicked = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("isHasButton = document.querySelector('#nextUser') != null;if(isHasButton){document.querySelector('#nextUser').click(); return true;};"));
            if (!buttonClicked)
            {
                driver.Navigate().GoToUrl("https://instagram.com/" + userName);
            }
            //new Instagram.SupportFunctions().IsPageLoaded();
            new Instagram.SupportFunctions().IsFollowUserPageLoaded();
            Thread.Sleep(1000);
            IJavaScriptExecutor isUserFindJS = (IJavaScriptExecutor)driver;
            bool isUserNotFind = driver.FindElements(By.XPath("//*[text()='Üzgünüz, bu sayfaya ulaşılamıyor.']")).Count > 0;
            User followUser = new User { _userName = userName, _followDate = DateTime.Now.ToShortDateString() };

            if (isUserNotFind)
            {
                kullanıcıBulunamadı += 1;
                Console.WriteLine("Bulunamayan Kullanıcıadı => " + userName);
                new OasisInstagram.Data.Transfer().ToFile("Bulunamayan Kullanıcıadı => " + userName);
                Thread.Sleep(180000);
            }
            else
            {
                kullanıcıBulundu += 1;
            }

            if (!isUserNotFind)
            {
                if (File.Exists(followPath))
                {
                    bool isPrivate = driver.FindElements(By.XPath("//*[text()='Bu Hesap Gizli']")).Count > 0;


                    var followBtn = driver.FindElements(By.XPath("//*[text()='Takip Et']"));
                    if (followBtn.Count > 0)
                    {
                        followBtn[0].Click();
                    }

                    if (!isPrivate)
                    {
                        string followedFileString = File.ReadAllText(followPath);
                        var followedUserDatas = JsonConvert.DeserializeObject<DataPatern>(followedFileString);
                        followedUserDatas._usersList?.Add(followUser);
                        var json = JsonConvert.SerializeObject(followedUserDatas);
                        File.WriteAllText(followPath, json);
                    }
                    else
                    {
                        WriteFollowRequestFile(userName);
                    }
                }
                else
                {
                    Console.WriteLine("FollowingDb.ozgur dosyası bulunamadı. Yenile butonuna tıklayın...");
                    new OasisInstagram.Data.Transfer().ToFile("FollowingDb.ozgur dosyası bulunamadı. Yenile butonuna tıklayın...");
                }
            }
            else
            {
                Console.WriteLine("Kullanıcı Bulunamadı => " + userName);
                new OasisInstagram.Data.Transfer().ToFile("Kullanıcı Bulunamadı => " + userName);
            }
        }
        public void AutoFollow(IWebDriver driver)
        {
            autoFollowRuning = true;
            string followPath = folderPath + @"/WillFollow.ozgur";
            if (File.Exists(followPath))
            {
                if (DailyFollowsOK())
                {
                    List<User> willFollow = new List<User>();

                    var followJsonString = File.ReadAllText(followPath);
                    var followJsonDatas = JsonConvert.DeserializeObject<DataPatern>(followJsonString);

                    if (followJsonDatas._usersList.Count > 0)
                    {
                        var script = "newDiv = document.createElement('div'); \n" +
                            $"newDiv.innerHTML = '<a id=\"nextUser\" href=\"/{followJsonDatas._usersList[0]._userName}/\" role=\"link\" tabindex=\"0\"></a>;' \n" +
                            "document.body.appendChild(newDiv);";
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                        FollowByUserName(followJsonDatas._usersList[0]._userName, driver);
                        followJsonDatas._usersList.Remove(followJsonDatas._usersList[0]);

                        string json = JsonConvert.SerializeObject(followJsonDatas);
                        File.WriteAllText(followPath, json);
                    }
                    willFollow.Clear();
                }
                else
                {
                    Console.WriteLine("Takip sınırına ulaşıldı");
                }
            }
            autoFollowRuning = false;
        }
        public void AutoUnfollow(IWebDriver driver)
        {
            autoUnfollowRuning = true;
            if (AutoUnfollowFirstRun)
            {
                WillUnfollow();
                AutoUnfollowFirstRun = false;
                Console.WriteLine("İlk çalışma tetiklendi");
                new OasisInstagram.Data.Transfer().ToFile("İlk çalışma tetiklendi");
            }
            string unFollowPath = folderPath + @"/WillUnfollow.ozgur";
            string unollowedPath = folderPath + @"/Unfollowed.ozgur";

            if (DailyUnfollowsOK() || !File.Exists(unollowedPath))
            {
                Console.WriteLine("Takipten Çıkma İşlemi Başladı");
                new OasisInstagram.Data.Transfer().ToFile("Takipten Çıkma İşlemi Başladı");

                List<User> willUnfollow = new List<User>();

                var unfollowJsonString = File.ReadAllText(unFollowPath);
                var unfollowJsonDatas = JsonConvert.DeserializeObject<DataPatern>(unfollowJsonString);

                if (unfollowJsonDatas._usersList.Count > 0)
                {

                    var script = "newDiv = document.createElement('div'); \n" +
                        $"newDiv.innerHTML = '<a id=\"nextUserUnfollow\" href=\"/{unfollowJsonDatas._usersList[0]._userName}/\" role=\"link\" tabindex=\"0\"></a>'; \n" +
                        "document.body.appendChild(newDiv);";
                    ((IJavaScriptExecutor)driver).ExecuteScript(script);

                    UnfollowByUserName(unfollowJsonDatas._usersList[0]._userName, driver);

                    unfollowJsonDatas._usersList.Remove(unfollowJsonDatas._usersList[0]);

                    string json = JsonConvert.SerializeObject(unfollowJsonDatas);
                    File.WriteAllText(unFollowPath, json);
                }
                willUnfollow.Clear();
            }
            else
            {
                Console.WriteLine("Takipten çıkma sınırına ulaşıldı");
                new OasisInstagram.Data.Transfer().ToFile("Takipten çıkma sınırına ulaşıldı");
            }
            autoUnfollowRuning = false;
        }
        //public async void UpdateFollowerAsync()
        //{
        //    List<User> users = new List<User>();
        //    User user;
        //    DataPatern dataPatern = new DataPatern();
        //    var followSend = await Form1.api.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(999999));
        //    var follower = followSend.Value.ToList();

        //    foreach (var item in follower)
        //    {
        //        user = new User();
        //        user._userName = item.UserName;
        //        user._followDate = DateTime.Now.ToShortDateString();
        //        users?.Add(user);
        //    }
        //    dataPatern._tableName = "Takipçi";
        //    dataPatern._usersList = users;

        //    Write(dataPatern, "FollowerDb.json");

        //}
        //public async void UpdateFollowAsync()
        //{
        //    List<User> users = new List<User>();
        //    User user;
        //    DataPatern dataPatern = new DataPatern();
        //    var apGet = await Form1.api.GetCurrentUserAsync();
        //    var followSend = await Form1.api.GetUserFollowingAsync(apGet.Value.UserName, PaginationParameters.MaxPagesToLoad(999999));
        //    var follow = followSend.Value.ToList();

        //    foreach (var item in follow)
        //    {
        //        user = new User();
        //        user._userName = item.UserName;
        //        user._followDate = DateTime.Now.ToShortDateString();
        //        users?.Add(user);
        //    }
        //    dataPatern._tableName = "Takip";
        //    dataPatern._usersList = users;

        //    Write(dataPatern, "FollowingDb.ozgur");

        //}
        public bool DailyUnfollowsOK()
        {
            string unfollowedPath = folderPath + @"/Unfollowed.ozgur";

            if (File.Exists(unfollowedPath))
            {
                string jsonSting = File.ReadAllText(unfollowedPath);
                var jsonDatas = JsonConvert.DeserializeObject<DataPatern>(jsonSting);
                var dailyUnfollow = jsonDatas._usersList.Where(x => x._followDate.Equals(DateTime.Now.ToShortDateString())).Count();
                Console.WriteLine("Bu gün unfollow sayısı: " + dailyUnfollow);
                new OasisInstagram.Data.Transfer().ToFile("Bu gün unfollow sayısı: " + dailyUnfollow);
                return dailyUnfollow <= 1000;
            }
            else
            {
                return false;
            }
        }
        public bool DailyFollowsOK()
        {
            string followedPath = folderPath + @"/FollowingDb.ozgur";
            if (File.Exists(followedPath))
            {
                int followLimit = 1000;
                string jsonSting = File.ReadAllText(followedPath);
                var jsonDatas = JsonConvert.DeserializeObject<DataPatern>(jsonSting);
                var dailyUnfollow = jsonDatas._usersList.Where(x => x._followDate.Equals(DateTime.Now.ToShortDateString())).Count();
                Console.WriteLine("Bu gün follow sayısı " + dailyUnfollow);
                new OasisInstagram.Data.Transfer().ToFile("Bu gün follow sayısı " + dailyUnfollow);
                return dailyUnfollow <= followLimit;
            }
            else
            {
                return false;
            }
        }
        public void GetFollowersByUserName(string userName, IWebDriver driver)
        {
            string path = folderPath + @"/WillFollow.ozgur";
            string jsonString2Data;
            List<User> users = new List<User>();
            List<User> previusUsers;

            IJavaScriptExecutor isPrivateUserJS = (IJavaScriptExecutor)driver;
            bool isPrivate = Convert.ToBoolean(isPrivateUserJS.ExecuteScript("return document.body.innerText.includes('Bu Hesap Gizli')"));

            IJavaScriptExecutor isUserFindJS = (IJavaScriptExecutor)driver;
            bool isUserFind = Convert.ToBoolean(isUserFindJS.ExecuteScript("return document.body.innerText.includes('Üzgünüz, bu sayfaya ulaşılamıyor.')"));

            if (!isUserFind || !isPrivate)
            {
                driver.Navigate().GoToUrl($"https://www.instagram.com/{userName}/followers/");
                new Instagram.SupportFunctions().IsPageLoaded(); // sayfa yüklenene kadar bekler

                ((IJavaScriptExecutor)driver).ExecuteScript(Properties.Resources.GetUsersFromUser);
                IJavaScriptExecutor followerJS = (IJavaScriptExecutor)driver;

                string getFollowers = null;
                bool isLoaded = false;
                while (!isLoaded)
                {
                    try
                    {
                        getFollowers = Convert.ToString(followerJS.ExecuteScript("return document.querySelector('#takipciler').innerText"));
                        if (getFollowers != null)
                        {
                            isLoaded = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                List<string> followerList = new List<string>();
                followerList.AddRange(getFollowers.Split(','));
                followerList.Remove("");

                if (File.Exists(path))
                {
                    string readJsonData = File.ReadAllText(path);
                    var followJsonString = File.ReadAllText(folderPath + @"/Unfollowed.ozgur");
                    var followerJsonString = File.ReadAllText(folderPath + @"/FollowRequest.ozgur");

                    var jsonData = JsonConvert.DeserializeObject<DataPatern>(readJsonData);
                    var jsonFollowDatas = JsonConvert.DeserializeObject<DataPatern>(followJsonString);
                    var jsonFollowerDatas = JsonConvert.DeserializeObject<DataPatern>(followerJsonString);


                    foreach (var username in followerList)
                    {
                        var sorgu = jsonData._usersList.Where(x => x._userName.Equals(username)).Count();
                        var sorguFollow = jsonFollowDatas._usersList.Where(x => x._userName.Equals(username)).Count();
                        var sorguFollower = jsonFollowerDatas._usersList.Where(x => x._userName.Equals(username)).Count();
                        if (sorgu < 1 && sorguFollow < 1 && sorguFollower < 1)
                        {
                            users?.Add(new User { _userName = username, _followDate = "ComingSoon" });
                        }
                    }

                    jsonData._usersList.AddRange(users);
                    jsonString2Data = JsonConvert.SerializeObject(jsonData);
                }
                else
                {
                    foreach (var username in followerList)
                    {
                        users?.Add(new User { _userName = username, _followDate = "ComingSoon" });
                    }

                    DataPatern dataPatern = new DataPatern();
                    dataPatern._tableName = "WillFollows";
                    dataPatern._usersList = users;
                    jsonString2Data = JsonConvert.SerializeObject(dataPatern);
                }

                File.WriteAllText(path, jsonString2Data);
            }
            else
            {
                MessageBox.Show("Kullanıcı adı yanlış veya hesap gizli olduğundan takipçileri alınamadı.", "Takipçiler Alınamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void WriteFollowRequestFile(string userName)
        {
            string followRequestPath = folderPath + @"/FollowRequest.ozgur";
            if (File.Exists(followRequestPath))
            {
                string followedFileString = File.ReadAllText(followRequestPath);
                var followedUserDatas = JsonConvert.DeserializeObject<DataPatern>(followedFileString);
                followedUserDatas._usersList?.Add(new User { _userName = userName, _followDate = DateTime.Now.ToShortDateString() });
                Console.WriteLine("Follow request: " + userName);
                new OasisInstagram.Data.Transfer().ToFile("Follow request: " + userName);
                var json = JsonConvert.SerializeObject(followedUserDatas);
                File.WriteAllText(followRequestPath, json);
            }
            else
            {
                List<User> newUser = new List<User>();
                DataPatern followRequestData = new DataPatern();
                followRequestData._tableName = "FollowRequest";
                User user = new User();
                user._userName = userName;
                user._followDate = DateTime.Now.ToShortDateString();
                newUser?.Add(user);
                followRequestData._usersList = newUser;
                var json = JsonConvert.SerializeObject(followRequestData);
                File.WriteAllText(followRequestPath, json);
            }
        }
        public void RemoveFollowRequestFile(string userName)
        {
            string followRequestPath = folderPath + @"/FollowRequest.ozgur";
            if (File.Exists(followRequestPath))
            {
                string followedFileString = File.ReadAllText(followRequestPath);
                var followRequestDatas = JsonConvert.DeserializeObject<DataPatern>(followedFileString);
                var user = followRequestDatas._usersList.Where(x => x._userName.Equals(userName)).FirstOrDefault();
                if (user != null)
                {
                    followRequestDatas._usersList.Remove(user);
                    var json = JsonConvert.SerializeObject(followRequestDatas);
                    File.WriteAllText(followRequestPath, json);
                    Console.WriteLine("Follow Request Dosyasından Silindi: " + userName);
                    new OasisInstagram.Data.Transfer().ToFile("Follow Request Dosyasından Silindi: " + userName);
                }
            }
        }
    }
}
