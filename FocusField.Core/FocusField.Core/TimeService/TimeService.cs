using System;
namespace FocusField.Core.TimeService
{
    public class TimeService : ITimeService
    {
        public DateTime GetTime() => DateTime.UtcNow;
    }
}
