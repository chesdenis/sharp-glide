using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Context;
using SharpGlide.Extensions;

namespace SharpGlide.Parts.Abstractions
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

            var children = new List<IBasePart>();
            
            EnumerateChildren( part =>
            {
                children.Add(part);
            });

            foreach (var basePart in children)
            {
                await basePart.StartAsync();
            }
        }

        public async Task StartAndStopAsync(TimeSpan onlinePeriod)
        {
            BackgroundExtensions.RunInParallel(onlinePeriod, StopAsync);
            await StartAsync();
        }

        public async Task StopAsync()
        {
            await Context.SwitchContext.TearDownAsync();
            
            var children = new List<IBasePart>();
            
            EnumerateChildren( part =>
            {
                children.Add(part);
            });

            foreach (var basePart in children)
            {
                await basePart.StopAsync();
            }
        }
        
        public TSettings GetSettings<TSettings>(string keyPath)
        {
            return Context.SettingsContext.GetByKey<TSettings>(keyPath);
        }
        
        public void ReportInfo(string status) => Context.MetaDataContext.Status["Info"] = status;
        
        public string GetStatusTable() => Context.HeartBeatContext.GetStatusTable(this);
        
        public Dictionary<string, string> Status => Context.MetaDataContext.Status;
        
        public void AddChild(IBasePart part) => Context.GroupContext.AddChild(part);

        public IBasePart GetChild(string name, bool recursive = false) => Context.GroupContext.GetChild(name, recursive);

        public void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false) =>
            Context.GroupContext.EnumerateChildren(partAction, recursive);
    }
}