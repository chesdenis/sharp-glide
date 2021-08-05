using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);
    }
}