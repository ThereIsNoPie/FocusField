using System;
using System.Collections.Generic;

namespace FocusField.Analytics.Models
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Combine(this IEnumerable<TimeSpan> times)
        {
            var totalTime = TimeSpan.Zero;
            foreach (TimeSpan currentValue in times)
            {
                totalTime += currentValue;
            }
            return totalTime;
        }
    }
}
