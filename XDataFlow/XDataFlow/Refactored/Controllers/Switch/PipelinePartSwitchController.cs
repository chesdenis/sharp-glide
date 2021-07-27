using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Exceptions;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Refactored.Controllers.Switch
{
    public class PipelinePartSwitchController<TConsumeData, TPublishData> : SwitchController
    {
        private readonly PipelinePart<TConsumeData, TPublishData> _pipelinePart;
        private readonly IConsumeController<TConsumeData> _consumeController;

        private CancellationTokenSource _cts;

        public PipelinePartSwitchController(
            PipelinePart<TConsumeData, TPublishData> pipelinePart, 
            IConsumeController<TConsumeData> consumeController)
        {
            _pipelinePart = pipelinePart;
            _consumeController = consumeController;
        }

        protected override async Task OnStartAsync()
        {
            _cts ??= new CancellationTokenSource();
            
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var tunnels = _consumeController.ConsumeTunnels;

                    foreach (var tunnelKey in tunnels.Keys)
                    {
                        await _pipelinePart.ProcessAsync(tunnels[tunnelKey].Consume(), _cts.Token);
                    }
                }
                catch (NoDataException)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        protected override Task OnStopAsync()
        {
            _cts.Cancel();

            return Task.CompletedTask;
        }
    }
}