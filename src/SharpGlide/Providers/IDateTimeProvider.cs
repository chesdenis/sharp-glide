using System;

namespace SharpGlide.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();
        DateTime GetNowUtc();
    }
}