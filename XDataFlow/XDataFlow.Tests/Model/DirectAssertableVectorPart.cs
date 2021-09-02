using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Registry;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
{
    public abstract class DirectAssertableVectorPart<TConsumeData, TPublishData> 
        : AssertableVectorPart<TConsumeData, TPublishData>
    {
        public override async Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
        {
            var publishData = Map(data);
                
            this.Publish(publishData);
                
            this.ConsumedData.Add(data);
            this.PublishedData.Add(publishData);

            await this.StopAsync();
        }
    }
}