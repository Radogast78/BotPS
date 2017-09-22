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
            String result=sendPost("http://elem.mobi", "plogin=" + this.textBox2.Text + "&ppass=" + this.textBox3.Text);
            this.richTextBox1.Text = result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String result = sendGet(this.textBox1.Text, "");
            this.richTextBox1.Text = result;

        }
    }
}
