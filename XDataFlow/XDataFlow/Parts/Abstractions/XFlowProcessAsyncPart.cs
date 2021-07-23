using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions.Lifecycle;

namespace XDataFlow.Parts.Abstractions.Async
{
    // public abstract class XFlowProcessAsyncPart<TConsumeData, TPublishData> : XFlowPartMeta
    // {
    //     private readonly IIPartLifecycle _lifecycle;
    //     private readonly CancellationToken _cancellationToken;
    //
    //     protected XFlowProcessAsyncPart(IIPartLifecycle lifecycle, CancellationToken cancellationToken)
    //     {
    //         _lifecycle = lifecycle;
    //         _cancellationToken = cancellationToken;
    //     }
    //     
    //     protected abstract Task Process(TConsumeData consumeData);
    // }
}