using System;

namespace SharpGlide.Parts
{
    public interface IPartReportSupport
    {
        void ReportInfo(string status);
        void Report(string key, string value);
        void ReportThreads(int threadsAmount);
        void ReportException(Exception ex);
    }
}