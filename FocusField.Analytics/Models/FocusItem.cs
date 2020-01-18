using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            var tempArray = Data;
            var toCombine = data;
            var combineable = tempArray.FirstOrDefault(x => x.CanCombine(toCombine));

            while (combineable != null)
            {
                tempArray = tempArray.Except(new List<FocusData>() { combineable });
                toCombine = combineable.Combine(toCombine).Value;
                combineable = tempArray.FirstOrDefault(x => x.CanCombine(toCombine));
            }

            Data = tempArray.Concat(new List<FocusData>() { toCombine });
        }
    }
}
