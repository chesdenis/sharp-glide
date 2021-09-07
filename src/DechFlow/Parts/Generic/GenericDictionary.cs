using System.Collections.Concurrent;
using DechFlow.Parts.Abstractions;
using DechFlow.Registry;

namespace DechFlow.Parts.Generic
{
    public abstract class GenericDictionary<TDictKey, TDictData> : PointPart
    {
        private readonly ConcurrentDictionary<TDictKey, TDictData> _dictionary = new ConcurrentDictionary<TDictKey, TDictData>();
        protected GenericDictionary(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }
        
        public void GetOrAdd(TDictKey key, TDictData data) => _dictionary.GetOrAdd(key, data);
    }
}