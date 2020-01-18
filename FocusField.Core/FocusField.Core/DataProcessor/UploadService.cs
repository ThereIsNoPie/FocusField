using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using FocusField.Core.TimeService;
using Newtonsoft.Json;

namespace FocusField.Core.DataProcessor
{
    public class UploadService : IUploadService
    {
        private static IEnumerable<FocusData> _storedData = new List<FocusData>();
        private readonly ITimeService _timeService;

        public UploadService(ITimeService timeService)
        {
            this._timeService = timeService;
        }


        private bool _isSending;
        private DateTime _lastSend = DateTime.MinValue;

        public async Task TrySendData()
        {
            var currentTime = _timeService.GetTime();

            if (_isSending || (currentTime - _lastSend) < TimeSpan.FromSeconds(5))
                return;
            _isSending = true;

            var toSend = _storedData.ToList();

            var didSend = await SendDataToBlob(toSend);

            if (didSend.IsSuccess)
            {
                _storedData = _storedData.Except(toSend);
                _lastSend = currentTime;
            }

            _isSending = false;
        }

        private async Task<Result> SendDataToBlob(IEnumerable<FocusData> datas)
        {
            var connectionString = "DefaultEndpointsProtocol=https;" +
               "AccountName=focusfield;" +
               "AccountKey=uVpWEWg1m9oX9fK2T3Ra3EQB5eZgsccKG9RkCCYjwD08yfOPq+fJoHDq1pSGO76fAXNdL1KCSCzH+sUBitkCZw==;" +
               "EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var containerName = "mitrealityhackdemo";
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var fileName = $"FromCore{Guid.NewGuid()}.txt";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            var sendJson = JsonConvert.SerializeObject(datas);

            byte[] byteArray = Encoding.ASCII.GetBytes(sendJson);
            MemoryStream stream = new MemoryStream(byteArray);

            try
            {
                await blobClient.UploadAsync(stream);
                return Result.Ok();
            }
            catch
            {

            }

            return Result.Failure("UploadAsync() threw an error");
        }

        public void StoreData(FocusData focusData)
        {
            _storedData = _storedData.Concat(new List<FocusData>() { focusData });
            TrySendData();
        }


    }
}
