using System;

namespace FocusField.Core.DataProcessor
{
    public class FocusDataDto
    {
        public FocusDataDto(DateTime startTime, DateTime endTime, int itemId)
        {
            StartTime = startTime;
            EndTime = endTime;
            ItemId = itemId;
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ItemId { get; set; }
    }
}
