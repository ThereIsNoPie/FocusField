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

            uploader.ProcessData(3);
            Thread.Sleep(500);
            uploader.ProcessData(3);
            Thread.Sleep(8000);
            uploader.ProcessData(1);
            Thread.Sleep(2000);
            uploader.ProcessData(1);
            Thread.Sleep(10000);
            uploader.ProcessData(3);


        }
    }
}
