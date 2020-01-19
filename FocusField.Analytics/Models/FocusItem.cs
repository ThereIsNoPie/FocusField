using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FocusField.Analytics.Models
{
    public class FocusItem 
    {
        IEnumerable<FocusData> Data { get; set; }
        public FocusItem(int itemId)
        {
            ItemId = itemId;
            Data = new List<FocusData>();
        }

        public int ItemId { get; }

        public TimeSpan TimeSpent => Data.Select(x => x.TimeSpent).Combine();


        public void AddAndCombineItem(FocusData data)
        {
            var gotCombined = new List<FocusData>();

            var combined = data;

            foreach (var item in Data)
            {
                if (item.CanCombine(combined))
                {
                    combined = item.Combine(combined).Value;
                    gotCombined.Add(item);
                }
            }

            Data = Data.Except(gotCombined)
                .Concat(new List<FocusData>() { combined });
        }
    }
}
