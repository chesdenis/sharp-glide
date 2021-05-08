using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Wrappers
{
    public static class WrapperExtensions
    {
        public static void AddStartWrapper<TWrapper>(this IPart part, Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapper 
        {
            part.StartWrappers.Add(wrapperPointer());
        }
        
        public static void AddStartWrapper<TWrapper>(this IPart part)
            where TWrapper : IWrapper, new()
        {
            part.StartWrappers.Add(new TWrapper());
        }

        public static void AddPublishWrapper<TWrapper, TPublishData>(this IPublishTunnel<TPublishData> tunnel,
            Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapperWithInput<TPublishData>
        {
            tunnel.OnPublishWrappers.Add(wrapperPointer());
        }
        
        public static void AddConsumeWrapper<TWrapper, TConsumeData>(this IConsumeTunnel<TConsumeData> tunnel,
            Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapperWithOutput<TConsumeData>
        {
            tunnel.OnConsumeWrappers.Add(wrapperPointer());
        }
    }
}