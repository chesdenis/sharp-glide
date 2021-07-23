using System;

namespace XDataFlow.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();

        DateTime GetNowUtc();
    }
}