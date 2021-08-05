using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Exceptions;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Refactored.Controllers.Switch
{
    public class VectorPartSwitchController<TConsumeData, TPublishData> : SwitchController
    {
        private readonly VectorPart<TConsumeData, TPublishData> _vectorPart;
        private readonly IConsumeController<TConsumeData> _consumeController;

        private CancellationTokenSource _cts;

        public VectorPartSwitchController(
            VectorPart<TConsumeData, TPublishData> vectorPart, 
            IConsumeController<TConsumeData> consumeController)
        {
            _vectorPart = vectorPart;
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
                    foreach (var data in _consumeController.ReadAndConsumeData())
                    {
                        await _vectorPart.ProcessAsync(data, _cts.Token);
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