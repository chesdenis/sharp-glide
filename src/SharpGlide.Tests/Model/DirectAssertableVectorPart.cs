using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model
{
    public abstract class DirectAssertableVectorPart<TConsumeData, TPublishData> 
        : AssertableVectorPart<TConsumeData, TPublishData>
    {
        public override async Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
        {
            var publishData = Map(data);
                
            Publish(publishData);
                
            ConsumedData.Add(data);
            PublishedData.Add(publishData);

            await StopAsync();
        }
    }
}