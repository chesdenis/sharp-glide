using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Registry;

namespace DechFlow.Parts.Generic
{
    // TODO: make buffer implementation
    public class GenericBuffer<TBufferData> : VectorPart<TBufferData, TBufferData[]>
    {
        private readonly List<TBufferData> _buffer = new List<TBufferData>();


        public GenericBuffer(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

        public override Task ProcessAsync(TBufferData data, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}