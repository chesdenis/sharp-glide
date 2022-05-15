using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Parts
{
    public interface ITransformPart<in TInput, TOutput> 
    {
        string Name { get; set; }

        public Task<TOutput> TransformAsync(TInput input, CancellationToken cancellationToken);
    }
}