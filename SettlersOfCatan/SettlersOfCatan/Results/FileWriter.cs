using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettlersOfCatan.Results
{
    public  class FileWriter
    {
        static ReaderWriterLock locker = new ReaderWriterLock();
        private static String directory = "default";
        public static void SaveResultToFile(string output)
        {
             
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                var path = AppDomain.CurrentDomain.BaseDirectory + "/Results/" + directory;
                var file = String.Format(path +  "/{0}.txt", (Thread.CurrentThread.ManagedThreadId*Program.executeGameNumber));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(output);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured {0}, during saving process", e.Message);
            }
            finally
            {
                locker.ReleaseReaderLock();
            }
        }

        public static void SetDirectory(string dir)
        {
            directory = dir;
        }
    }
}
