using System;

namespace DechFlow.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();

        DateTime GetNowUtc();
    }
}