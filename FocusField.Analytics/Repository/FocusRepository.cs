using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FocusField.Analytics.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace FocusField.Analytics.Repository
{
    public class FocusRepository
    {
        //private static IEnumerable<FocusItem> _data = new List<FocusItem>();
        //public IEnumerable<FocusItem> GetAll() => _data;

        private List<FocusItem> AddDataToCollection(
            FocusData data, List<FocusItem> allData)
        {
            if (allData.Any(x => x.ItemId == data.ItemId))
            {
                allData.First(x => x.ItemId == data.ItemId).AddAndCombineItem(data);
            }
            else
            {
                var focusItem = new FocusItem(data.ItemId);
                focusItem.AddAndCombineItem(data);
                allData.Add(focusItem);
            }

            return allData;
        }

        public async Task<IEnumerable<FocusItem>> GetFocusDataFromServer()
        {
            var connectionString = "DefaultEndpointsProtocol=https;" +
                           "AccountName=focusfield;" +
                           "AccountKey=uVpWEWg1m9oX9fK2T3Ra3EQB5eZgsccKG9RkCCYjwD08yfOPq+fJoHDq1pSGO76fAXNdL1KCSCzH+sUBitkCZw==;" +
                           "EndpointSuffix=core.windows.net";
            var containerName = "mitrealityhackdemo";
            var account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(containerName);

            BlobContinuationToken continuationToken = null;

            var httpClient = new HttpClient();
            List<FocusItem> returnedItems = new List<FocusItem>();
            do
            {
                try
                {
                    var resultSegment = await container.ListBlobsSegmentedAsync(
                        prefix: null,
                        useFlatBlobListing: true,
                        BlobListingDetails.All,
                        maxResults: 1000,
                        continuationToken,
                        options: null,
                        operationContext: null);

                    continuationToken = resultSegment.ContinuationToken;
                    foreach (var blob in resultSegment.Results)
                    {
                        Console.WriteLine("asdf");
                        var jsonBlobData = await httpClient.GetStringAsync(blob.Uri);

                        var dtos = JsonConvert.DeserializeObject<IEnumerable<FocusDataDto>>(jsonBlobData);

                        var domainData = dtos.Select(x => x.ToDomain());

                        foreach (var data in domainData)
                        {
                            returnedItems = AddDataToCollection(data, returnedItems);
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            while (continuationToken != null);

            return returnedItems;

            //BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //var container = blobServiceClient.GetBlobContainerClient(containerName);
            //var blobs = container.GetBlobs();
            //var blob = blobs.First();
            //blob.Metadata.Keys.First();

            //var blobsEnumerator = container.GetBlobsAsync().GetAsyncEnumerator();
            //while (await blobsEnumerator.MoveNextAsync())
            //{
            //    var jsonBlobData = blobsEnumerator.Current;
            //    Console.WriteLine("Asdf");
            //    //var data = JsonConvert.DeserializeObject<IEnumerable<FocusDataDto>>(jsonBlobData);
            //}
        }
    }
}
