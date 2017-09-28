using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Windows.Forms;

namespace BotPS
{
    public partial class Form1 : Form
    {
        public CookieContainer cookies;
        public Form1()
        {
            this.cookies = new CookieContainer();
            InitializeComponent();
        }
        public void ErrorLog(String userName,Exception e,String page)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\" + userName + ".log", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(e);
                sw.WriteLine(page);
            }

        }
        public String sendPost(String url, String post)
        {
            String result = "";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(post);
            System.Threading.Thread.Sleep(1000);
            HttpWebRequest postRequest = (HttpWebRequest)WebRequest.Create(url);
            postRequest.CookieContainer = this.cookies;
            postRequest.Method = "POST";
            postRequest.ContentType = "application/x-www-form-urlencoded";
            postRequest.ContentLength = data.Length;
            Stream postStream = postRequest.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            postStream.Close();
            HttpWebResponse postResponse = (HttpWebResponse)postRequest.GetResponse();
            postStream = postResponse.GetResponseStream();
            StreamReader sr = new StreamReader(postStream);
            result = sr.ReadToEnd();
            sr.Close();
            postStream.Close();
            //cookies = postRequest.CookieContainer;
            return result;
        }
        public String sendGet(String urlGet, String data)
        {
            String result = "";
            System.Threading.Thread.Sleep(1000);
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(urlGet);
            getRequest.CookieContainer = cookies;
            getRequest.Method = "GET";
            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            Stream getStream = getResponse.GetResponseStream();
            StreamReader sr = new StreamReader(getStream);
            result = sr.ReadToEnd();
            sr.Close();
            getStream.Close();
            return result;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            HtmlAgilityPack.HtmlDocument page=new HtmlAgilityPack.HtmlDocument(); ;
            try
            {

                String result = sendPost("http://elem.mobi", "plogin=" + this.textBox2.Text + "&ppass=" + this.textBox3.Text);
                page.LoadHtml(result);
                //< div class="ml5 mr3 pt2"> Получаем блок статистики
                //String login = page.DocumentNode.SelectNodes("//div[@class=\"wr7\"]")[0].InnerText;
                //MessageBox.Show(login);
                //String allStats = page.DocumentNode.SelectNodes("//div[@class=\"ml5 mr3 pt2\"]")[0].InnerHtml;
                /*        < span class="fr"><img src = "/img/ico16-silver.png" width="16" height="16" alt="">
                 *        <span class="c_silver">10.49M</span> <img src = "/img/ico16-gold.png" width="16" height="16" alt="">
                 *        <span class="c_gold">1176</span></span>
                 *        <img src = "/img/ico16-sword-white.png" width="16" height="16" alt="">
                 *        <span class="c_da">51 284</span>*/
                //HtmlAgilityPack.HtmlDocument stats = new HtmlAgilityPack.HtmlDocument();
                //stats.LoadHtml(allStats);
                //String silver = stats.DocumentNode.SelectNodes("//span[@class=\"c_silver\"]")[0].InnerText;
                //String gold = stats.DocumentNode.SelectNodes("//span[@class=\"c_gold\"]")[0].InnerText;
                //String strength = stats.DocumentNode.SelectNodes("//span[@class=\"c_da\"]")[0].InnerText;
                //this.label1.Text = "Серебро: "+silver;
                //this.label2.Text = "Золото: "+gold;
                //this.label3.Text = "Сила колоды: "+strength;
                String profilePage = sendGet("http://elem.mobi/profile/", "");
                HtmlAgilityPack.HtmlDocument profile = new HtmlAgilityPack.HtmlDocument();
                profile.LoadHtml(profilePage);//<div class="ml5 mr3 pt2">
                String statistics= profile.DocumentNode.SelectNodes("//div[@class=\"ml5 mr3 pt2\"]")[0].InnerHtml;
                HtmlAgilityPack.HtmlDocument stats = new HtmlAgilityPack.HtmlDocument();
                stats.LoadHtml(statistics);
                String diamond = stats.DocumentNode.SelectNodes("//span[@class=\"c_energy\"]")[0].InnerText;
                String silver= stats.DocumentNode.SelectNodes("//span[@class=\"c_silver\"]")[0].InnerText;
                String gold = stats.DocumentNode.SelectNodes("//span[@class=\"c_gold\"]")[0].InnerText;
                String strength = stats.DocumentNode.SelectNodes("//span[@class=\"c_da\"]")[0].InnerText;

                this.label1.Text = "Алмазы: "+diamond;
                this.label2.Text = "Серебро: "+silver;
                this.label3.Text = "Золото: "+gold;
                this.label6.Text = "Сила колоды: "+strength;

                /*                int length = stats.DocumentNode.SelectNodes("//span[@class=\"c_da\"]").Count;
                                String all = "";
                                for(int i = 0; i < length; i++)
                                {
                                    all=all+stats.DocumentNode.SelectNodes("//span[@class=\"c_da\"]")[i].InnerText+"\n";
                                }*/
                //<span class="c_da">Ubez</span>
                //this.richTextBox1.Text = all;
            }
            catch(Exception ex)
            {
                this.ErrorLog(this.textBox2.Text, ex, page.DocumentNode.InnerHtml);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String result = sendGet(this.textBox1.Text, "");
            this.richTextBox1.Text = result;

        }
    }
}
