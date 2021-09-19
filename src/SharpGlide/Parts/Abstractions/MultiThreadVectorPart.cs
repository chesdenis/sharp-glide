using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class MultiThreadVectorPart<TConsumeData, TPublishData>
        : VectorPart<TConsumeData, TPublishData>
    {
        private readonly ConcurrentDictionary<Guid,Task> _backgroundTasks = new ConcurrentDictionary<Guid,Task>();

        public abstract int GetApproxMaxThreads();

        public abstract Task MultiThreadProcessAsync(TConsumeData data, CancellationToken cancellationToken);

        public override Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (GetNotFinishedThreadsCount() > GetApproxMaxThreads())
                {
                    Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).Wait(cancellationToken);
                }
                else
                {
                    var tasksToRemove = GetFinishedThreads();
                    foreach (var task in tasksToRemove)
                    {
                        _backgroundTasks.TryRemove(task.Key, out _);
                    }
                    break;
                }
            }
            
            ReportThreads(GetNotFinishedThreadsCount());

            _backgroundTasks.TryAdd(Guid.NewGuid(), Task.Run(async () =>
            {
                await MultiThreadProcessAsync(data, cancellationToken);
            }, cancellationToken));
            
            return Task.CompletedTask;
        }

        private KeyValuePair<Guid, Task>[] GetFinishedThreads()
        {
            return _backgroundTasks.Where(w => TaskFinished(w.Value.Status)).ToArray();
        }

        private int GetNotFinishedThreadsCount()
        {
            return _backgroundTasks.Values.Count(w => !TaskFinished(w.Status));
        }

        private bool TaskFinished(TaskStatus status)
        {
            return status == TaskStatus.Faulted
                   |
                   status == TaskStatus.RanToCompletion
                   |
                   status == TaskStatus.Canceled;
        }
    }
}