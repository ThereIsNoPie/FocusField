using System;
namespace FocusField.Core.TimeService
{
    public interface ITimeService
    {
        DateTime GetTime();
    }

    public class MockTimeService : ITimeService
    {
        DateTime _storedTime;
        public MockTimeService()
        {
            _storedTime = DateTime.UtcNow;
        }

        public DateTime GetTime()
        {
            _storedTime = _storedTime.AddSeconds(6);
            return _storedTime;
        }
    }
}
