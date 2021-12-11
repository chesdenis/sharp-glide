using System;
using System.Collections.Generic;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Extensions
{
    public static class ConsumeWrappersExtensions
    {
        public static void AddConsumeWrapper<T>(this IList<IConsumeWrapper<T>> consumeWrappers,
            Func<IConsumeWrapper<T>> wrapperImpl)
        {
            consumeWrappers.Add(wrapperImpl?.Invoke());
        }

        public static void AddConsumeWrapper<T, TWrapper>(this IList<IConsumeWrapper<T>> consumeWrappers)
            where TWrapper : IConsumeWrapper<T>, new()
        {
            consumeWrappers.Add(new TWrapper());
        }
    }
}