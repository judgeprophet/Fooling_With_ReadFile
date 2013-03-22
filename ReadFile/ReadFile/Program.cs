using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReadFile
{
    public class Program
    {
	    //Main Fonction
		//Encore des commentaires
		//GIT
        static void Main(string[] args)
        {

            Console.WriteLine("Debut:" + DateTime.Now);

            string filename = args[0];
            string filter = args[1];
            bool error = false;

            Console.WriteLine("Filename:" + filename);
            Console.WriteLine("filter:" + filter);


            if (!File.Exists(filename))
            {
                Console.WriteLine("File does not exist.");
                error = true;
            }

            string[] filters = filter.Split(new char[] { ',' });

            if (!error)
            {
                filterTokens(filename, filters);
            }

            Console.WriteLine("Fin:" + DateTime.Now);
        }


        public static void filterTokens(string path, string[] filters)
        {

            int[] result;
            
            result = new int[filters.Length];

            using (FileStream fs = File.OpenRead(path))
            {
                //Get to the top
                fs.Seek(0, SeekOrigin.Begin);
                
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);


                //Read File
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    string stringBlock = temp.GetString(b);

                    for (int i = 0; i < filters.Length; i++)
                    {
                        //RegPattern
                        string pat = "\\b(" + filters[i] + ")\\b";
                        
                        // Instantiate the regular expression object.
                        Regex rex = new Regex(pat, RegexOptions.IgnoreCase);
                        
                        // Match the regular expression pattern against a text string.
                        MatchCollection mc = rex.Matches(stringBlock);
                        result[i] += mc.Count;

                    }
                }
            }

            for (int i = 0; i < filters.Length; i++)
            {
                Console.WriteLine(filters[i] + ":" + result[i]);
            }



            }


    }
}
