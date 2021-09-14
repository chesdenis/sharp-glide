using System.Collections.Concurrent;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Parts.Generic
{
    public abstract class GenericDictionary<TDictKey, TDictData> : PointPart
    {
        private readonly ConcurrentDictionary<TDictKey, TDictData> _dictionary = new ConcurrentDictionary<TDictKey, TDictData>();
        protected GenericDictionary() : base()
        {
        }
        
        public void GetOrAdd(TDictKey key, TDictData data) => _dictionary.GetOrAdd(key, data);
    }
}