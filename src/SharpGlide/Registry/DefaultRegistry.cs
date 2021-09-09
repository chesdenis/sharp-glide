using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpGlide.Registry
{
    public class DefaultRegistry : IDefaultRegistry
    {
        private const string DefaultName = "_Default";
        
        private readonly ConcurrentDictionary<Type,
            Dictionary<string, Func<object>>> _bag = 
            new ConcurrentDictionary<Type, Dictionary<string, Func<object>>>();

        public void Set<T>(Func<object> builder)
        {
            SetNamed<T>(builder, DefaultName);
        }

        public void SetNamed<T>(Func<object> builder, string name)
        {
            lock (_bag)
            {
                if(_bag.TryGetValue(typeof(T), out var dict))
                {
                    dict[name] = builder;
                }
                else
                {
                    _bag[typeof(T)] = new Dictionary<string, Func<object>>
                    {
                        [name] = builder
                    };
                }
            }
        }

        public T Get<T>()
        {
            return GetNamed<T>(DefaultName);
        }

        public T GetNamed<T>(string name)
        {
            lock (_bag)
            {
                return  (T)_bag[typeof(T)][name]();
            }
        }
    }
}