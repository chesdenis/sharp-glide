using System;
using System.Collections.Generic;
using SharpGlide.Wrappers;

namespace SharpGlide.Extensions
{
    public static class PublishWrapperExtensions
    {
        public static void AddInputWrapper<T>(this IList<IPublishWrapper<T>> inputWrappers,
            Func<IPublishWrapper<T>> wrapperImpl)
        {
            inputWrappers.Add(wrapperImpl?.Invoke());
        }
    }
}