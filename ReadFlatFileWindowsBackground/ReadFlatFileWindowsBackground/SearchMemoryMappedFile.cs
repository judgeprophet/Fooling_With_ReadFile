using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication1
{
    public class SearchMemoryMappedFile
    {
        // Object to store the current state, for passing to the caller.
        public class CurrentState
        {
            public int LinesCounted;
            public int WordsMatched;
        }

        public CurrentState currentState = new CurrentState();
        private int _wordCount;
        private int _lineCount;

        public string SourceFile;
        public string CompareString;
        
        public void CountWords(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
                //public void CountWords()
        {
            var state = SearchInFile();
            worker.ReportProgress(0, state);
        }


        public void SearchInFileBlock(Stream mmvStream, byte[] b, long index, int sizeToRead, long maxIndex)
        {
            var mms = mmvStream;
            Console.WriteLine("Debut: {0}", DateTime.Now.ToString("hh:mm:ss.ff"));
            //CurrentState state = new CurrentState();
            ASCIIEncoding temp = new ASCIIEncoding();

            var wordcount = 0;
            var linecount = 0;

            mms.Position = index;
            while (mms.Read(b, 0, sizeToRead) > 0 && index < maxIndex)
            {
                String line = temp.GetString(b);
                wordcount += StringHelper.Instance.CountInString(line, CompareString);
                linecount++;

                index = mms.Position;
            }

            currentState.WordsMatched = wordcount;
            currentState.LinesCounted = linecount;

            Console.WriteLine("Index : {2} _ maxI:{3} _ Word:{0} Line:{1}", wordcount, linecount, index, maxIndex);
            //return state;
            Console.WriteLine("Fin: {0}", DateTime.Now.ToString("hh:mm:ss.ff"));

        }

        public CurrentState SearchInFile()
        {
            // Initialize the variables.
            CurrentState state = new CurrentState();

            ////===== 30Sec ===========================
            //using (StreamReader sr = File.OpenText(SourceFile))
            //{
            //    string line = String.Empty;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        _wordCount += StringHelper.Instance.CountInString(line, CompareString);
            //        _lineCount++;
            //    }
            //}

            //===== 25sec ================================
            //using (FileStream fs = File.Open(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (BufferedStream bs = new BufferedStream(fs))
            //using (StreamReader sr = new StreamReader(bs))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        _wordCount += StringHelper.Instance.CountInString(line, CompareString);
            //        _lineCount++;
            //    }
            //}

            //===== 10 sec MemoryMappedFile ================================
            //==== Sans le CountInString == 2sec
            byte[] b = new byte[32768]; //5sec
            int sizeToRead = b.Length;
            using (MemoryMappedFile mmFile = MemoryMappedFile.CreateFromFile(SourceFile))
            {
                using (Stream mmvStream = mmFile.CreateViewStream())
                {
                    ASCIIEncoding temp = new ASCIIEncoding();
                    while (mmvStream.Read(b, 0, sizeToRead) > 0)
                    {
                        String line = temp.GetString(b);
                        _wordCount += StringHelper.Instance.CountInString(line, CompareString);
                        _lineCount++;
                    }
                }
            }

            //===== MemoryMappedFile Thread ================================
            //// Initialize the variables.
            //byte[] b = new byte[32768]; //5sec
            //int sizeToRead = b.Length;
            //long maxIndex = 0;
            ////MemoryMappedFile mmFile = MemoryMappedFile.CreateFromFile(SourceFile);
            //using (MemoryMappedFile mmFile = MemoryMappedFile.CreateFromFile(SourceFile))
            //{

            //    Stream mmvStream = mmFile.CreateViewStream();
            //    BufferedStream bs = new BufferedStream(mmvStream);

            //    //using (Stream mmvStream = mmFile.CreateViewStream()) //ne peut utilise le using dans ce contexte car le thread a besoin de mmvStream
            //    {
            //        //Premiere Partie a tester

            //        var chunk = bs.Length;/// 4;

            //        long index = 0; // total number of bytes read
            //        maxIndex = chunk;
            //        Thread t = new Thread(th => SearchInFileBlock(bs, b, index, sizeToRead, maxIndex));
            //        t.Start();

            //        //long index2 = (int)maxIndex;
            //        //var maxIndex2 = chunk * 2;
            //        //Thread t2 = new Thread(th => SearchInFileBlock(bs, b, index2, sizeToRead, maxIndex2));
            //        //t2.Start();

            //        //long index3 = (int)maxIndex2;
            //        //var maxIndex3 = chunk * 3;
            //        //Thread t3 = new Thread(th => SearchInFileBlock(bs, b, index3, sizeToRead, maxIndex3));
            //        //t3.Start();                   

            //    }
            //}

            state.WordsMatched = _wordCount;
            state.LinesCounted = _lineCount;

            return state;
        }
    }
}
    
