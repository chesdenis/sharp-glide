using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Exceptions;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Context
{
    public class VectorPartSwitchContext<TConsumeData, TPublishData> : SwitchContext
    {
        private readonly VectorPart<TConsumeData, TPublishData> _vectorPart;
        private readonly IConsumeContext<TConsumeData> _consumeContext;

        public VectorPartSwitchContext(
            VectorPart<TConsumeData, TPublishData> vectorPart, 
            IConsumeContext<TConsumeData> consumeContext)
        {
            _vectorPart = vectorPart;
            _consumeContext = consumeContext;
        }

        protected override async Task OnStartAsync()
        {
            while (true)
            {
                if (_vectorPart.GetExecutionTokenSource().Token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    foreach (var data in _consumeContext.ReadAndConsumeData())
                    {
                        await _vectorPart.ProcessAsync(data, _vectorPart.GetExecutionTokenSource().Token);
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
            _vectorPart.GetExecutionTokenSource().Cancel();

            return Task.CompletedTask;
        }
    }
}