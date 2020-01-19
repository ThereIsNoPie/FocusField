using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using FocusField.Analytics.Models;

namespace FocusField.Analytics.Services
{
    public class OneSecondData : ValueObject
    {
        public OneSecondData(int itemId, DateTime startPoint)
        {
            ItemId = itemId;
            StartPoint = startPoint;
        }

        public int ItemId { get; }
        public DateTime StartPoint { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ItemId;
            yield return StartPoint;
        }
    }

    public class DataFormattor
    {
        public IEnumerable<OneSecondData> GetTimeMap(IEnumerable<FocusItem> items)
        {
            var startTime = items
                .Select(x => x.Earliest)
                .Min();
            var endTime = items
                .Select(x => x.Latest)
                .Max();

            var returnData = new List<OneSecondData>(); 

            if (!startTime.HasValue || !endTime.HasValue)
            {
                return returnData;
            }

            var time = startTime.Value;


            while (time < endTime)
            {
                var inFocusAtTime = items.FirstOrDefault(x => x.IsInItems(time));
                if (inFocusAtTime != null)
                {
                    var oneSecondData = new OneSecondData(
                        inFocusAtTime.ItemId, time);
                    returnData.Add(oneSecondData);
                }
                time = time.AddSeconds(1);
            }

            return returnData;
        }

    }
}
