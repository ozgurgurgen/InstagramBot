using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OasisInstagram
{
    public partial class Form1 : Form
    {
        private bool processingFinished;
        bool clickedShowPassword = false;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            timer2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread task = new Thread(InstagramLogin);
            InstagramSettings.Default.instagramUserName = userNameTBX.Text;
            InstagramSettings.Default.instagramPassword = passwordTBX.Text;
            InstagramSettings.Default.Save();
            task.Start();

        }

        private void InstagramLogin()
        {
            new Classes.Instagram.Main().Start(userNameTB.Text);
            processingFinished = true;
            Console.WriteLine("İşlem Bitti");
        }

        private void followButton_Click(object sender, EventArgs e)
        {
            /*timer1.Interval = 100;
            //timer2.Interval = 10000;
            timer1.Start();
           // timer2.Start();
            Console.WriteLine(timer1.Enabled);
            Console.WriteLine(timer1.Interval);
            Console.WriteLine(Classes.Data.Json.autoFollowRuning);
            Console.WriteLine(Classes.Data.Json.autoUnfollowRuning);
            
           // new Classes.Instagram.Main().Deneme(userNameTB.Text);*/

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tatiklendi 1");
            new OasisInstagram.Data.Transfer().ToFile("Tatiklendi 1");
            if (processingFinished && !Classes.Data.Json.autoFollowRuning && !Classes.Data.Json.autoUnfollowRuning)
            {
                new Classes.Data.Json().AutoFollow(Classes.Instagram.Main.driver);
            }
            timer1.Interval = new Random().Next(45000, 120000);
            Console.WriteLine("Atomatik Takip İşlemi Tetiklenme Süresi => " + timer1.Interval / 1000 + " Saniye");
            new OasisInstagram.Data.Transfer().ToFile("Atomatik Takip İşlemi Tetiklenme Süresi => " + timer1.Interval / 1000 + " Saniye");
            FolAndUnfDelay();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tatiklendi 2");
            new OasisInstagram.Data.Transfer().ToFile("Tatiklendi 2");

            if (processingFinished && !Classes.Data.Json.autoFollowRuning && !Classes.Data.Json.autoUnfollowRuning)
            {
                new Classes.Data.Json().AutoUnfollow(Classes.Instagram.Main.driver);
            }

            timer2.Interval = new Random().Next(45000, 120000);
            Console.WriteLine("Atomatik Takibi Bırakma İşlemi Tetiklenme Süresi => " + timer2.Interval / 1000 + " Saniye");
            new OasisInstagram.Data.Transfer().ToFile("Atomatik Takibi Bırakma İşlemi Tetiklenme Süresi => " + timer2.Interval / 1000 + " Saniye");
            FolAndUnfDelay();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            bool driverRunning = Classes.Instagram.Main.driver != null;
            if (driverRunning)
            {
                Classes.Instagram.Main.driver.Quit();
            }
            Application.Exit();
            Environment.Exit(0);
            //var ss = Classes.Instagram.Main.driver.Manage().Window.;
        }

        private void FolAndUnfDelay()
        {
            if (Classes.Data.Json.kullanıcıBulunamadı >= 2)
            {
                Classes.Data.Json.kullanıcıBulundu = 0;
                timer1.Interval = 1800000;
                timer2.Interval = 1810000;
                Console.WriteLine("Bulunamayan kullanıcı sayısı 2'e ulaştı. Tüm İşlemler 30 dakika ertelendi.");
                new OasisInstagram.Data.Transfer().ToFile("Bulunamayan kullanıcı sayısı 2'e ulaştı. Tüm İşlemler 30 dakika ertelendi.");
                Classes.Data.Json.kullanıcıBulunamadı = 0;

            }

            if (Classes.Data.Json.kullanıcıBulundu > 0)
            {
                Classes.Data.Json.kullanıcıBulunamadı = 0;
                Classes.Data.Json.kullanıcıBulundu = 0;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new ConsoleScreen().Show();
            userNameTBX.Text = InstagramSettings.Default.instagramUserName;
            passwordTBX.Text = InstagramSettings.Default.instagramPassword;
        }

        private void label4_Click(object sender, EventArgs e)
        {            
            if (!clickedShowPassword)
            {
                passwordTBX.PasswordChar = '\0';
                clickedShowPassword = true;
            }
            else
            {
                passwordTBX.PasswordChar = '*';
                clickedShowPassword = false;
            }
        }
    }
}
