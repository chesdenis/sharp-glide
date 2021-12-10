using System;
using System.Collections.Generic;
using SharpGlide.Wrappers;

namespace SharpGlide.Extensions
{
    public static class ConsumeWrappersExtensions
    {
        public static void AddOutputWrapper<T>(this IList<IConsumeWrapper<T>> outputWrappers,
            Func<IConsumeWrapper<T>> wrapperImpl)
        {
            outputWrappers.Add(wrapperImpl?.Invoke());
        }
    }
}