using System;
using System.Collections.Generic;

namespace XDataFlow.Registry
{
    public class XFlowDefaultRegistry
    {
        public static readonly Dictionary<Type, Func<object>> DefaultImpls = new Dictionary<Type, Func<object>>();

        public static void Set<T>(Func<object> builder)
        {
            DefaultImpls[typeof(T)] = builder;
        }
        
        public static T Get<T>() => (T)DefaultImpls[typeof(T)]();
    }
}