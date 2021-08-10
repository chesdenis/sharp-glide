using System;
using System.Threading.Tasks;
using XDataFlow.Context;
using XDataFlow.Controllers.Switch.Behaviours;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class BasePart : IBasePart
    {
        public IPartContext Context { get; set; }

        public string Name
        {
            get => Context.MetaDataController.Name;
            set => Context.MetaDataController.Name = value;
        }

        public void ConfigureStartAs<TBehaviour>(Func<TBehaviour> behaviourFunc)
            where TBehaviour : IStartBehaviour
        {
            Context.SwitchController.StartBehaviour = behaviourFunc();
        }

        public void ConfigureStartAs<TBehaviour>()
            where TBehaviour : IStartBehaviour, new()
        {
            Context.SwitchController.StartBehaviour = new TBehaviour();
        }

        public void ConfigureStopAs<TBehaviour>(TBehaviour behaviour)
            where TBehaviour : IStopBehaviour
        {
            Context.SwitchController.StopBehaviour = behaviour;
        }

        public void ConfigureStopAs<TBehaviour>()
            where TBehaviour : IStopBehaviour, new()
        {
            Context.SwitchController.StopBehaviour = new TBehaviour();
        }

        public async Task StartAsync()
        {
            await Context.SwitchController.TearUpAsync();
        }

        public async Task StopAsync()
        {
            await Context.SwitchController.TearDownAsync();
        }

        public void PrintStatus(string status) => this.Context.MetaDataController.Status["InProgress"] = status;
    }
}