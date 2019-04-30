using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox1.KeyDown += (s, e) =>
            {
                if (e.KeyData == Keys.Return)
                {
                    Button2_Click(null, null);
                }
            };

            TextBox1.TextChanged += (s, e) =>
            {
                if (Regex.IsMatch(TextBox1.Text, "[^0-9-.]"))
                    TextBox1.Text = TextBox1.Text.Remove(TextBox1.Text.Length - 1);
                TextBox1.SelectionStart = TextBox1.Text.Length;
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //887; 491 def
            Size = new Size(370, 364);
        }

        async void Button2_Click(object sender, EventArgs e)
        {
            TextBox1.Enabled = false;
            Button2.Enabled = false;
            await Task.Run(() => {
                WebClient wb = new WebClient();
            string line = wb.DownloadString($"http://free.ipwhois.io/xml/{TextBox1.Text}");
            Debug.WriteLine(line);
            if (!line.Contains("invalid"))
            {
                Action action = () => {
                    Match match = Regex.Match(line, "<country>(.*?)</country>(.*?)<region>(.*?)</region>(.*?)<city>(.*?)</city>(.*?)<org>(.*?)</org>(.*?)<currency>(.*?)</currency>");
                    Match matchloc = Regex.Match(line, "<latitude>(.*?)</latitude>(.*?)<longitude>(.*?)</longitude>");
                    Label8.Text = match.Groups[1].ToString();
                    Label9.Text = match.Groups[3].ToString();
                    Label10.Text = match.Groups[5].ToString();
                    Label11.Text = match.Groups[7].ToString();
                    Label12.Text = match.Groups[9].ToString();
                    string latitude = matchloc.Groups[1].ToString();
                    string longitude = matchloc.Groups[3].ToString();
                    Size = new Size(1180, 656);
                    WebBrowser1.Url = new Uri($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}");
                    //maps(latitude, longitude);
                    Debug.WriteLine(latitude + ":" + longitude);
                };

                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
                    
            }
            else
            {
                MessageBox.Show("Загугли, как выглядит IP");
                }
            });
            TextBox1.Enabled = true;
            Button2.Enabled = true;
        }

        /*async void maps(string latitude, string longitude)
        {
            await Task.Run(() => {
                WebBrowser1.Url = new Uri($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}");
            });
        }*/
    }
}
