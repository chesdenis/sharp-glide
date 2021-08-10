using System.Threading;
using System.Threading.Tasks;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);
    }
}