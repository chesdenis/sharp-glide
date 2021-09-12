using System;
using System.Threading.Tasks;
using SharpGlide.Exceptions;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context.Switch
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
                var cancellationTokenSource = GetExecutionTokenSource();
                
                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    foreach (var data in _consumeContext.ReadAndConsumeData())
                    {
                        await _vectorPart.ProcessAsync(data, cancellationTokenSource.Token);
                    }
                }
                catch (NoDataException)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationTokenSource.Token);
                }
            }
        }

        protected override Task OnStopAsync()
        {
            GetExecutionTokenSource().Cancel();

            return Task.CompletedTask;
        }
    }
}