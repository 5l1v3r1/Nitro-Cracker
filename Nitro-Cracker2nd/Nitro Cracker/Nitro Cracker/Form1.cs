using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Leaf.xNet;

namespace Nitro_Cracker
{
    public partial class Form1 : Form
    {
        public static List<string> Proxies = new List<string>();
        public static bool FoundCode = false;
        public static Random rnd = new Random();
        public static int Checked_C = 0;
        public static int Errors = 0;
        public static ProxyType proxytype;
        public static object _lock = new object();
        public Form1()
        {
            //Checkcode("", );
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedIndex == 0)
                proxytype = ProxyType.HTTP;

            else if (metroComboBox1.SelectedIndex == 1)
                proxytype = ProxyType.Socks4;

            else if (metroComboBox1.SelectedIndex == 2)
                proxytype = ProxyType.Socks5;

            else
            {
                MessageBox.Show("Please select a proxy type");
                return;
            }

            int threadcount = trackBar1.Value;
            new Thread(delegate ()
            {
                while (!FoundCode)
                {
                    List<Thread> threads = new List<Thread>();

                    for (int l = 0; l < threadcount; l++)//SETTINGS
                    {

                        Thread t = new Thread(delegate ()
                        {
                            while (!FoundCode)
                            {
                                string code = GetCode();
                                bool ok = false;
                                var watch = System.Diagnostics.Stopwatch.StartNew();
                                while (!ok)
                                {
                                    UpdateValues();


                                    int linecount = 0;
                                    richTextBox1.Invoke(new MethodInvoker(delegate () { linecount = richTextBox1.Lines.Count(); }));

                                    if (linecount > 500)
                                    {

                                        richTextBox1.Invoke(new MethodInvoker(delegate () { richTextBox1.Lines.ToList().Clear(); }));
                                    }

                                    if (Proxies.Count() < 30)
                                    {
                                        Proxies = Load(@"./Proxies.txt");

                                    }

                                    try
                                    {
                                        string proxyaddy;
                                        lock (_lock)
                                        {
                                            proxyaddy = Proxies.ElementAt(rnd.Next(0, Proxies.Count() - 1));
                                        }

                                        ok = Checkcode(code, proxyaddy);
                                    }
                                    catch (Exception)
                                    {
                                        //outToLog(e.Message);
                                    }

                                    UpdateValues();



                                }
                                Checked_C++;
                                watch.Stop();
                                var elapsedMs = watch.ElapsedMilliseconds;

                                if (Proxies.Count() < 30)
                                {
                                    Proxies = Load(@"./Proxies.txt");
                                }

                                try
                                {
                                    //Globals.Last_50_Checktimes.Add(elapsedMs);

                                    //Globals.CPM = (int)(Config.Threads * 60000 / Globals.Last_50_Checktimes.Average());
                                }
                                catch (Exception)
                                {
                                    //outToLog(e.Message);
                                }
                                UpdateValues();
                            }
                        });
                        t.Start();
                        threads.Add(t);
                        Thread.Sleep(100);
                    }
                    foreach (Thread thread in threads.ToList())
                    {

                        thread.Join();
                        threads.Remove(thread);
                    }

                }
            }).Start();
        }
        public static List<string> Load(string dir)
        {
            List<string> toLoad = new List<string>();

            foreach (string line in File.ReadLines(dir))
            {
                toLoad.Add(line.Replace("\n", ""));

            }
            return toLoad.Distinct().ToList();
        }
        private void UpdateValues()
        {
            label3.Invoke(new MethodInvoker(delegate () { label3.Text = "Tested: " + Checked_C; label4.Text = "Errors: " + Errors; }));
        }
        public static string GetCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnopqrstuvwxyzZ0123456789";
            return new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public bool Checkcode(string code, string proxyaddy)
        {

            try
            {
                using (var request = new HttpRequest())
                {
                    if (proxytype == ProxyType.Socks5)
                    {
                        request.Proxy = Socks5ProxyClient.Parse(proxyaddy);
                    }
                    else if (proxytype == ProxyType.Socks4)
                    {
                        request.Proxy = Socks4ProxyClient.Parse(proxyaddy);
                    }
                    else
                    {
                        request.Proxy = HttpProxyClient.Parse(proxyaddy);
                    }

                    string text  = request.Get("https://discordapp.com/api/v6/entitlements/gift-codes/" + code).ToString();
                    FoundCode = true;
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    outToLog("Found Code: " + code);
                    //MessageBox.Show("You found a nitro code : " + code);

                }
                return true;
            }
            catch (HttpException ex)
            {
                if ( (int)ex.HttpStatusCode == 404)
                {
                    outToLog("Bad code " + code);

                    return true;
                    
                }
                else
                {
                    //outToLog("removed proxy " + proxyaddy);
                    
                    Errors++;
                    return false;
                }
            }
            
        }

        public void outToLog(string output)
        {
            richTextBox1.Invoke(new MethodInvoker(delegate () {
                richTextBox1.AppendText("\r\n" + output);
                richTextBox1.ScrollToCaret();
            }));

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = "Threads: " + trackBar1.Value;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
