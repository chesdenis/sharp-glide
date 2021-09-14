using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Parts.Generic
{
    // TODO: make buffer implementation
    public class GenericBuffer<TBufferData> : VectorPart<TBufferData, TBufferData[]>
    {
        private readonly List<TBufferData> _buffer = new List<TBufferData>();


        public GenericBuffer() : base()
        {
        }

        public override Task ProcessAsync(TBufferData data, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}