using System;

namespace SharpGlide.Providers
{
    public class DateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;

        public DateTime GetNowUtc() => DateTime.UtcNow;
        
        internal static IDateTimeProvider CustomProvider { get; set; }
        
        public static DateTime Now => CustomProvider?.GetNow() ?? DateTime.Now;
        public static DateTime NowUtc => CustomProvider?.GetNowUtc() ?? DateTime.UtcNow;
    }
}