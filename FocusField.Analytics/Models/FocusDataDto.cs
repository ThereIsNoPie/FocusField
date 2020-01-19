using System;

namespace FocusField.Analytics.Models
{
    public class FocusDataDto
    {
        public FocusDataDto(DateTime startTime, DateTime endTime, int itemId)
        {
            StartTime = startTime;
            EndTime = endTime;
            ItemId = itemId;
        }

        public string ToFormattedString() =>
            $"StartTime = {StartTime}\n" +
            $"EndTime = {EndTime}\n" +
            $"ItemId = {ItemId}\n";


        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ItemId { get; set; }
    }

    public static class FocusDataDtoExtensions
    {
        public static FocusData ToDomain(this FocusDataDto dto) =>
            new FocusData(dto.StartTime, dto.EndTime, dto.ItemId);
    }
}
