using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web提交测试
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句

            string url = textBox4.Text;
            try
            {
                if (comboBox1.Text == "POST")
                    textBox3.Text = GetPost(url, textBox2.Text, "application/x-www-form-urlencoded");
                else
                    textBox3.Text = GetHtml(url, comboBox2.Text);

            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                textBox3.Text = ex.Message + "\r\n\r\n" + result;
            }

        }

        /// <summary>
        /// GET方式请求Html页面
        /// </summary>
        public static string GetHtml(string url, string CharacterSet)
        {
            StreamReader sr = null;
            string str = null;
            //读取远程路径
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            //request.Timeout = Timeout;
            request.Accept = "image/gif,   image/x-xbitmap,   image/jpeg,   image/pjpeg,   application/x-shockwave-flash,   application/vnd.ms-excel,   application/vnd.ms-powerpoint,   application/msword,   */* ";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream st = response.GetResponseStream();
            if (response.ContentEncoding.ToLower() == "gzip")
                st = new GZipStream(st, CompressionMode.Decompress);

            sr = new StreamReader(st, Encoding.GetEncoding(CharacterSet));
            str = sr.ReadToEnd();
            sr.Close();
            return str;
        }


        /// <summary>
        /// POST提交，并获取返回结果
        /// </summary>
        public static string GetPost(string Url, string Content, string ContentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = ContentType;

            StreamWriter myWriter = new StreamWriter(request.GetRequestStream());
            myWriter.Write(Content);
            myWriter.Close();

            HttpWebResponse objResponse = (HttpWebResponse)request.GetResponse();
            string result = "";
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }


    }
}
