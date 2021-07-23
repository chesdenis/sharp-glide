using System;

namespace XDataFlow.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;

        public DateTime GetNowUtc() => DateTime.UtcNow;
    }
}