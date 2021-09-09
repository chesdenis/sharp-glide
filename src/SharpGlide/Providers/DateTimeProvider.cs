using System;

namespace SharpGlide.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;

        public DateTime GetNowUtc() => DateTime.UtcNow;
    }
}