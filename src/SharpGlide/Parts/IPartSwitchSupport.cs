using System;
using System.Threading.Tasks;

namespace SharpGlide.Parts
{
    public interface IPartSwitchSupport
    {
        Task StartAsync();
        Task StartAsync(TimeSpan onlinePeriod);
        Task StartRegular(TimeSpan interval, TimeSpan onlinePeriod);
        Task StopAsync();
        Task StopAsync(TimeSpan stopAfter);
    }
}