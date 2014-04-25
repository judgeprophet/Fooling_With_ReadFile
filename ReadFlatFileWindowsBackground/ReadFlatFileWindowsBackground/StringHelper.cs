using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Singleton
    /// </summary>
    sealed class StringHelper
    {
        /// <summary>
        /// Allocate ourselves.
        /// We have a private constructor, so no one else can.
        /// </summary>
        static readonly StringHelper _instance = new StringHelper();

        /// <summary>
        /// Access StringHelper.Instance to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static StringHelper Instance
        {
            get { return _instance; }
        }

        private StringHelper()
        {
        }

        /// <summary>
        /// This function counts the number of times a word is found in a line.
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="CompareString"></param>
        /// <returns></returns>
        public int CountInString(string SourceString, string CompareString)
        {
            
            if (String.IsNullOrEmpty(SourceString))
            {
                return 0;
            }

            //int count = new Regex(Regex.Escape(CompareString)).Matches(SourceString).Count;

            ////================== Straight for-loop
            //string[] ss = new[]{SourceString};
            //string[] sf = new[]{CompareString};
            //int count = 0;
            //for (int x = 0; x < ss.Length; x++)
            //{
            //}
            ////========================


            ////================== Parallel.For 8 sec
            object lockObject = new object();
            string[] ss = new[] { SourceString };
            string[] sf = new[] { CompareString };
            //char[] ch =   new[]{Convert.ToChar(CompareString)};
            int total = 0;
            Parallel.For(0, ss.Length,
                            () => 0,
                            (x, loopState, subtotal) =>
                            {
                                for (int y = 0; y < sf.Length; y++)
                                {
                                    // if string in the string +1 (like string present == true)
                                    //subtotal += ((ss[x].Length - ss[x].Replace(sf[y], String.Empty).Length) / sf[y].Length > 0 ? 1 : 0); //number of line
                                    subtotal += ((ss[x].Length - ss[x].Replace(sf[y], String.Empty).Length) / sf[y].Length); 
                                }
                                return subtotal;
                            },
                            (s) =>
                            {
                                lock (lockObject)
                                {
                                    total += s;
                                }
                            }
                        );
            int count = total;
            //========================

            //string EscapedCompareString =
            //    System.Text.RegularExpressions.Regex.Escape(CompareString);

            //System.Text.RegularExpressions.Regex regex;
            //regex = new System.Text.RegularExpressions.Regex(
            //    EscapedCompareString,
            //    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //System.Text.RegularExpressions.MatchCollection matches;
            //matches = regex.Matches(SourceString);
            //count = matches.Count;
            
            
            return count;
        }
    }
}
