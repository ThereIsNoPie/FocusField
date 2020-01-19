using System;
using FocusField.Analytics.Repository;
using System.Threading.Tasks;

using FocusField.Analytics.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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
            var repository = new FocusRepository();
            var allData = await repository.GetFocusDataFromServer();

            foreach (var item in allData)
            {
                Console.WriteLine($"{item.ItemId}: {item.TimeSpent}");
            }
        }
    }
}
