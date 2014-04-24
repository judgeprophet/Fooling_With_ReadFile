using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace WindowsFormsApplication1
{
    public class Words
    {
        // Object to store the current state, for passing to the caller.
        public class CurrentState
        {
            public int LinesCounted;
            public int WordsMatched;
        }

        public string SourceFile;
        public string CompareString;
        public int FilePart;
        public int FileSplitPart;
        private int WordCount;
        private int LinesCounted;


        public void CountWords(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        //public void CountWords()
        {
            // Initialize the variables.
            CurrentState state = new CurrentState();
            string line = "";
            int elapsedTime = 20;
            DateTime lastReportDateTime = DateTime.Now;

            if (CompareString == null ||
                CompareString == System.String.Empty)
            {
                throw new Exception("CompareString not specified.");
            }

            using (FileStream fs = File.OpenRead(SourceFile))
            {

                byte[] b = new byte[32768]; //5sec
                //byte[] b = new byte[131072]; //5sec
                //byte[] b = new byte[65536]; //5sec
                UTF8Encoding temp = new UTF8Encoding(true);


                int length = (int)fs.Length;  // get file length
                //b = new byte[length];       // create buffer
                int count = 0;                // actual number of bytes read
                int sum = 0;                  // total number of bytes read

                //Read File upper
                //int max = (int)(fs.Length / 2);

                //Get to the top
                fs.Seek(0, SeekOrigin.Begin);

                while (fs.Read(b, sum, b.Length) > 0)
                {
                    //sum += count;
                    //if (worker.CancellationPending)
                    //{
                    //    e.Cancel = true;
                    //    break;
                    //}
                    //else
                    //{
                    line = temp.GetString(b);
                    WordCount += StringHelper.Instance.CountInString(line, CompareString);
                    LinesCounted += 1;

                    // Raise an event so the form can monitor progress.
                    int compare = DateTime.Compare(DateTime.Now, lastReportDateTime.AddMilliseconds(elapsedTime));
                    if (compare > 0)
                    {
                        state.LinesCounted = LinesCounted;
                        state.WordsMatched = WordCount;
                        //worker.ReportProgress(0, state);
                        lastReportDateTime = DateTime.Now;
                    }
                    //}

                    // Uncomment for testing.
                    //System.Threading.Thread.Sleep(5);
                }

            }

            // Report the final count values.
            state.LinesCounted = LinesCounted;
            state.WordsMatched = WordCount;
            worker.ReportProgress(0, state);
        }



    }
}
