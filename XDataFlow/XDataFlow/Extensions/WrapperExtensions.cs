using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Extensions
{
    public static class WrapperExtensions
    {
        public static void AddWrapper<TWrapper, TIn>(this PublisherOnlyFlowPart<TIn> part, Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapper 
        {
            part.OnEntryWrappers.Add(wrapperPointer());
        }
        
        public static void AddWrapper<TWrapper, TIn, TOut>(this PublisherConsumerFlowPart<TIn, TOut> part, Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapper 
        {
            part.OnEntryWrappers.Add(wrapperPointer());
        }

        
        public static void AddWrapper<TWrapper, TIn>(this PublisherOnlyFlowPart<TIn> part)
            where TWrapper : IWrapper, new()
        {
            part.OnEntryWrappers.Add(new TWrapper());
        }
        
        public static void AddWrapper<TWrapper, TIn, TOut>(this PublisherConsumerFlowPart<TIn, TOut> part)
            where TWrapper : IWrapper, new()
        {
            part.OnEntryWrappers.Add(new TWrapper());
        }

        public static void AddWrapper<TWrapper, TIn>(this IPublishTunnel<TIn> tunnel,
            Func<TWrapper> wrapperPointer)
            where TWrapper : IWrapperWithInput<TIn>
        {
            tunnel.OnPublishWrappers.Add(wrapperPointer());
        }
    }
}