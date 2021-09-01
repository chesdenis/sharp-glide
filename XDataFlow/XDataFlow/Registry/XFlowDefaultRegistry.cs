using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XDataFlow.Registry
{
    public class XFlowDefaultRegistry
    {
        public static readonly Lazy<ConcurrentDictionary<Type, Func<object>>> LazyFactory =
            new Lazy<ConcurrentDictionary<Type, Func<object>>>(() => new ConcurrentDictionary<Type, Func<object>>());

        public static ConcurrentDictionary<Type, Func<object>> Current => LazyFactory.Value;

        private XFlowDefaultRegistry()
        {
        }

        public static void Set<T>(Func<object> builder)
        {
            Current[typeof(T)] = builder;
        }
        
        public static T Get<T>() => (T)Current[typeof(T)]();
    }
}