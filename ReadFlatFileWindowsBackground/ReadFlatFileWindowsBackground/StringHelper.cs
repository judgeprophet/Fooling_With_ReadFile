using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int CountInString(string SourceString, string CompareString)
        {
            // This function counts the number of times
            // a word is found in a line.
            if (SourceString == null)
            {
                return 0;
            }

            string EscapedCompareString =
                System.Text.RegularExpressions.Regex.Escape(CompareString);

            System.Text.RegularExpressions.Regex regex;
            regex = new System.Text.RegularExpressions.Regex(
                EscapedCompareString,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Text.RegularExpressions.MatchCollection matches;
            matches = regex.Matches(SourceString);
            return matches.Count;
        }
    }
}
