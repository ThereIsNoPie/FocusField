using System;
using FocusField.Core.TimeService;

namespace FocusField.Core.DataProcessor
{
    public interface IDataProcessor
    {
        // 0 = nothing
        // 1 = book
        // 2 = phone (distracting)
        void ProcessData(int lookingId);
    }

    public class DataProcessor : IDataProcessor
    {
        private readonly ITimeService _timeService;
        private readonly IUploadService _uploadService;
        private DateTime _previousTime;
        private int _previousId;

        public DataProcessor(
            ITimeService timeService,
            IUploadService uploadService)
        {
            this._timeService = timeService;
            this._uploadService = uploadService;
        }

        public void ProcessData(int itemId)
        {
            var currentTime = _timeService.GetTime();

            if (_previousTime == null)
            {
                _previousTime = currentTime;
                return;
            }

            if (itemId == _previousId && currentTime - _previousTime < TimeSpan.FromSeconds(5))
                return;

            var focusData = new FocusData(_previousTime, currentTime, _previousId);
            _uploadService.StoreData(focusData);

            _previousTime = currentTime;
            _previousId = itemId;
        }
    }
}
