using System;
using System.Threading.Tasks;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Extensions;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class BasePart : IBasePart
    {
        public IPartContext Context { get; set; }

        public string Name
        {
            get => Context.MetaDataContext.Name;
            set => Context.MetaDataContext.Name = value;
        }

        public void ConfigureStartAs<TBehaviour>(Func<TBehaviour> behaviourFunc)
            where TBehaviour : IStartBehaviour
        {
            Context.SwitchContext.StartBehaviour = behaviourFunc();
        }

        public void ConfigureStartAs<TBehaviour>()
            where TBehaviour : IStartBehaviour, new()
        {
            Context.SwitchContext.StartBehaviour = new TBehaviour();
        }

        public void ConfigureStopAs<TBehaviour>(TBehaviour behaviour)
            where TBehaviour : IStopBehaviour
        {
            Context.SwitchContext.StopBehaviour = behaviour;
        }

        public void ConfigureStopAs<TBehaviour>()
            where TBehaviour : IStopBehaviour, new()
        {
            Context.SwitchContext.StopBehaviour = new TBehaviour();
        }

        public async Task StartAsync()
        {
            await Context.SwitchContext.TearUpAsync();
        }

        public async Task StartAndStopAsync(TimeSpan onlinePeriod)
        {
            BackgroundExtensions.RunInParallel(onlinePeriod, StopAsync);
            await StartAsync();
        }

        public async Task StopAsync()
        {
            await Context.SwitchContext.TearDownAsync();
        }

        public void PrintStatus(string status) => this.Context.MetaDataContext.Status["InProgress"] = status;
    }
}