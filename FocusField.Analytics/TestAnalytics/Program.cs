using System;
using FocusField.Analytics.Repository;
using System.Threading.Tasks;
using FocusField.Analytics.Services;
using System.Linq;
using System.Collections.Generic;
using FocusField.Analytics.Models;
using System.IO;

namespace TestAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            TestApiAsync().Wait();

            while (true)
            {
                var s = Console.ReadLine();
                if (s != "y")
                    break;
                TestApiAsync().Wait();
            }
        }

        private static async Task TestApiAsync()
        {
            var repository = new DownloadService();
            var allData = await repository.GetFocusDataFromServer();

            WriteToConsole(allData);
            WriteToFile(allData);
            
        }



        private static void WriteToFile(IEnumerable<FocusItem> allData)
        {
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            using (StreamWriter outputFile = new StreamWriter(
                Path.Combine(docPath, "FocusTimeSpent.csv"), false))
            {
                foreach (var item in allData)
                {
                    outputFile.WriteLine($"{item.ItemId},{StringFromId(item.ItemId)},{item.TimeSpent}");
                }
            }

            var dataFormattor = new DataFormattor();
            var timeMap = dataFormattor.GetTimeMap(allData);
            using (StreamWriter outputFile = new StreamWriter(
                Path.Combine(docPath, "SecondBySecond.csv"), false))
            {
                foreach (var oneSecondData in timeMap)
                {
                    outputFile.WriteLine($"{oneSecondData.StartPoint},{StringFromId(oneSecondData.ItemId)},{oneSecondData.ItemId}");
                }
            }

            var sortedTimeData = allData
                .SelectMany(x => x.Data)
                .OrderBy(x => x.StartTime);
            using (StreamWriter outputFile = new StreamWriter(
                Path.Combine(docPath, "ActivityByActivity.csv"), false))
            {
                foreach (var focus in sortedTimeData)
                {
                    outputFile.WriteLine($"{focus.ItemId},{StringFromId(focus.ItemId)},{focus.TimeSpent.TotalSeconds}");
                }
            }
            using (StreamWriter outputFile = new StreamWriter(
                 Path.Combine(docPath, "AllData.csv"), false))
            {
                foreach (var focus in sortedTimeData)
                {
                    outputFile.WriteLine($"{focus.ItemId},{StringFromId(focus.ItemId)},{focus.StartTime},{focus.EndTime}");
                }
            }
        }

        private static void WriteToConsole(IEnumerable<FocusItem> allData) {
            foreach (var item in allData)
            {
                Console.WriteLine($"{StringFromId(item.ItemId)}: {item.TimeSpent}");
            }

            Console.WriteLine("\n\n>>>>>>> length of time");
            var sortedTimeData = allData
                .SelectMany(x => x.Data)
                .OrderBy(x => x.StartTime);
            foreach (var focus in sortedTimeData)
            {
                Console.WriteLine($"{StringFromId(focus.ItemId)} : for {focus.TimeSpent.TotalSeconds} seconds");
            }

            Console.WriteLine("\n\n>>>>>> Longest Unbroken time");
            var largest = sortedTimeData
                .OrderBy(x => x.TimeSpent)
                .Where(x => x.ItemId != 0)
                .LastOrDefault();
            Console.WriteLine($"Longest Focus Time = {largest.TimeSpent.TotalSeconds} seconds " +
                $"for { StringFromId(largest.ItemId)}");
        }

        private static string StringFromId(int id) => ((IdToObject)id).ToString();
    }
}
