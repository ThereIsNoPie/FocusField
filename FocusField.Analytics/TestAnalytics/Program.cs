using System;
using FocusField.Analytics.Repository;
using System.Threading.Tasks;
using FocusField.Analytics.Services;
using System.Linq;

namespace TestAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            TestApiAsync().Wait();
        }

        private static async Task TestApiAsync()
        {
            var repository = new DownloadService();
            var allData = await repository.GetFocusDataFromServer();

            foreach (var item in allData)
            {
                Console.WriteLine($"{item.ItemId}: {item.TimeSpent}");
            }


            Console.WriteLine(">>>>>>>> Second By Second graph");
            var dataFormattor = new DataFormattor();

            var timeMap = dataFormattor.GetTimeMap(allData);
            foreach (var onSecondData in timeMap)
            {
                Console.WriteLine($"{onSecondData.StartPoint} : {onSecondData.ItemId}");
            }



            Console.WriteLine(">>>>>>> length of time");
            var sortedTimeData = allData.SelectMany(x => x.Data)
                .OrderBy(x=>x.StartTime);

            foreach (var focus in sortedTimeData){
                Console.WriteLine($"Object Id {focus.ItemId} : for {focus.TimeSpent.TotalSeconds} seconds");
            }
            

        }
    }
}
