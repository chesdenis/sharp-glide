using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Exceptions;
using XDataFlow.Parts.Abstractions.Sync;

namespace XDataFlow.Parts.Abstractions.Lifecycle
{
    // public class XFlowProcessLifecycle<TConsumeData, TPublishData> : XFlowLifecycle
    // {
    //     private readonly XFlowProcessPart<TConsumeData, TPublishData> _processPart;
    //
    //     private CancellationTokenSource _cts;
    //     
    //     public XFlowProcessLifecycle(XFlowProcessPart<TConsumeData, TPublishData> processPart)
    //     {
    //         _processPart = processPart;
    //     }
    //     
    //     protected override void OnStart()
    //     {
    //         _cts = new CancellationTokenSource();
    //         
    //         while (true)
    //         {
    //             if (_cts.Token.IsCancellationRequested)
    //             {
    //                 break;
    //             }
    //
    //             try
    //             {
    //                 // TODO: Uncomment this
    //                 // this.Consume<IFlowConsumerPart<TConsumeData>, TConsumeData>(ProcessMessage);
    //                 // lastConsume = DateTime.Now;
    //             }
    //             catch (NoDataException)
    //             {
    //                 Task.Delay(TimeSpan.FromSeconds(1)).Wait();
    //             }
    //         }
    //     }
    //
    //     protected override void OnStop()
    //     {
    //         _cts.Cancel();
    //     }
    // }
}