using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CalcTotalDownloadSize
{
    class Program
    {
        static void Main(string[] args)
        {
            long totalBytes = 0;
            var fileLocation = ConfigurationManager.AppSettings["DownloadUrlFileLocation"];
            IEnumerable<string> files = null;
            if (!File.Exists(fileLocation))
            {
                Console.WriteLine("DownloadUrlFileLocation file does not exist!");
            }

            files = File.ReadLines(fileLocation);
            foreach (var file in files)
            {
                Console.WriteLine("Getting info for {0}", file);

                var webRequest = HttpWebRequest.Create(new Uri(file));
                webRequest.Method = "HEAD";

                using (var webResponse = webRequest.GetResponse())
                {
                    var fileSize = webResponse.Headers.Get("Content-Length");
                    totalBytes += Convert.ToInt64(fileSize);
                    Console.WriteLine("File Size {0}", fileSize);
                }
            }

            var fileSizeInMegaByte = Math.Round(Convert.ToDouble(totalBytes) / 1024.0 / 1024.0, 2);
            Console.WriteLine("All files operations done. Total Download Size {0} MB.", fileSizeInMegaByte);
            Console.ReadLine();
        }

        private static string GetFileSize(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                var fileSizeInMegaByte = Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
                return fileSizeInMegaByte + " MB";
            }
        }
    }
}
