using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public abstract Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken);
    }
}