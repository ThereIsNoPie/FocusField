using System;
using System.Threading;
using Azure.Storage.Blobs.Models;
using FocusField.Core.DataProcessor;
using FocusField.Core.TimeService;

namespace FocusField.ManualTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var uploader = new DataProcessor(
                new TimeService(),
                new UploadService(new TimeService()));

            while(true)
            {
                var s = Console.ReadLine();
                var didParse = Int32.TryParse(s, out var val);

                if (!didParse)
                    break;
                uploader.ProcessData(val);
            }

        }
    }
}
