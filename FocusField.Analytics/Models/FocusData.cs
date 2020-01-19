using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace FocusField.Analytics.Models
{
    public class FocusData : ValueObject
    {
        public FocusData(DateTime startTime, DateTime endTime, int ownerId)
        {
            StartTime = startTime;
            EndTime = endTime;
            ItemId = ownerId;
        }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public int ItemId { get; }

        public TimeSpan TimeSpent => EndTime - StartTime;

        public bool IsInTimeSpan(DateTime time) =>
            time >= StartTime && time <= EndTime;

        public bool CanCombine(FocusData data) =>
            data.ItemId == ItemId
            && (data.EndTime == StartTime || data.StartTime == EndTime);

        public Maybe<FocusData> Combine(FocusData data) =>
            CanCombine(data) ?
                new FocusData(
                    new DateTime(Math.Min(data.StartTime.Ticks, StartTime.Ticks)),
                    new DateTime(Math.Max(data.EndTime.Ticks, EndTime.Ticks)),
                    ItemId)
                : null;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
            yield return EndTime;
            yield return ItemId;
        }
    }

}
