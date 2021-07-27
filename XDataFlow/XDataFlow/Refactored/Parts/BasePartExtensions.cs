using System;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Switch.Behaviours;

namespace XDataFlow.Refactored.Parts
{
    public static class BasePartExtensions
    {
        public static void ConfigureStartAs<TBehaviour>(this BasePart part, Func<TBehaviour> behaviourFunc)
            where TBehaviour : IStartBehaviour
        {
            part.SwitchController.StartBehaviour = behaviourFunc();
        }
        
        public static void ConfigureStartAs<TBehaviour>(this BasePart part)
            where TBehaviour : IStartBehaviour, new()
        {
            part.SwitchController.StartBehaviour = new TBehaviour();
        }
        
        public static void ConfigureStopAs<TBehaviour>(this BasePart part, TBehaviour behaviour)
            where TBehaviour : IStopBehaviour
        {
            part.SwitchController.StopBehaviour = behaviour;
        }
        
        public static void ConfigureStopAs<TBehaviour>(this BasePart part)
            where TBehaviour : IStopBehaviour, new()
        {
            part.SwitchController.StopBehaviour = new TBehaviour();
        }

        public static async Task StartAsync(this BasePart part)
        {
            await part.SwitchController.StartAsync();
        }

        public static async Task StopAsync(this BasePart part)
        {
            await part.SwitchController.StopAsync();
        }
    }
}