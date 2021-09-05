using System;
using System.Threading.Tasks;

namespace XDataFlow.Extensions
{
    public static class BackgroundExtensions
    {
        public static void RunInParallel(TimeSpan delayBeforeRun, params Func<Task>[] taskToRun)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delayBeforeRun);
                foreach (var task in taskToRun)
                {
                    await task();
                }
            });
        }
    }
}