using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace FocusField.Core.DataProcessor
{

    public static class FocusDataExtensions
    {
        public static IEnumerable<FocusData> AddOrCombine(
            this IEnumerable<FocusData> list, FocusData focusData)
        {
            var combineable = list.FirstOrDefault(x => x.CanCombine(focusData));
            return combineable != null
                ? list.Except(new List<FocusData>() { combineable })
                    .Concat(new List<FocusData>() { combineable.Combine(focusData).Value })
                : list.Concat(new List<FocusData>() { focusData });
        }
    }

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

        public bool CanCombine(FocusData data) =>
            data.ItemId == ItemId
            && ( data.EndTime == StartTime || data.StartTime == EndTime);

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
