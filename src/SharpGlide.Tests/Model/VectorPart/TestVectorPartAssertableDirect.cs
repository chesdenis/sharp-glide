using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.VectorPart
{
    public abstract class TestVectorPartAssertableDirect<TConsumeData, TPublishData> 
        : TestVectorPartAssertable<TConsumeData, TPublishData>
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