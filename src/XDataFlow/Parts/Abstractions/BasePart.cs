using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Extensions;
using XDataFlow.Renders;

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

            var children = new List<IBasePart>();
            
            this.EnumerateChildren( part =>
            {
                children.Add(part);
            }, false);

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
            
            this.EnumerateChildren( part =>
            {
                children.Add(part);
            }, false);

            foreach (var basePart in children)
            {
                await basePart.StopAsync();
            }
        }
        
        public TSettings GetSettings<TSettings>(string keyPath)
        {
            return Context.SettingsContext.GetByKey<TSettings>(keyPath);
        }
        
        public void ReportInfo(string status) => this.Context.MetaDataContext.Status["Info"] = status;

        // TODO: Console.Write(statusTextBuilder.ToString());
        // TODO: File.WriteAllText("PartStatus.txt", statusTextBuilder.ToString());
        // TODO: Implement console redirection component
        // public static void RedirectStatusToConsole(Timer timer, IEnumerable<IPart> parts)
        // {
        //     RenderToConsole(parts);
        //     timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
        //     timer.Start();
        //
        //     timer.Elapsed += (sender, eventArgs) => RenderToConsole(parts);
        // }
        public string GetStatusTable() => this.Context.HeartBeatContext.GetStatusTable(this);
        
        public Dictionary<string, string> Status => this.Context.MetaDataContext.Status;
        
        public void AddChild(IBasePart part) => Context.GroupContext.AddChild(part);

        public IBasePart GetChild(string name, bool recursive = false) => this.Context.GroupContext.GetChild(name, recursive);

        public void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false) =>
            this.Context.GroupContext.EnumerateChildren(partAction, recursive);
    }
}