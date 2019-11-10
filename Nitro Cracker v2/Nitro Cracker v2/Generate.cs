using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitro_Cracker_v2
{
    class Generate
    {
        public static Random rnd = new Random();

        public static List<string> generate(int count)
        {
            List<string> Generated_Codes = new List<string>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnopqrstuvwxyzZ0123456789";
            string progress = "--------------------"; //len 20
            StringBuilder sb = new StringBuilder(progress);
            

            while (Generated_Codes.Count() != count)
            {
                string code = new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());

                //if (!Generated_Codes.Contains(code))
                //    Generated_Codes.Add(code);

                Generated_Codes.Add(code);

                try
                {
                    sb[Generated_Codes.Count() * progress.Length / count] = '#';

                }
                catch { }

                Console.Write("\rGenerating codes [" + progress + "] " + Generated_Codes.Count() + "/"+ count);

                progress = sb.ToString();

            }
            Console.WriteLine();
            return Generated_Codes;
        }
    }
}
