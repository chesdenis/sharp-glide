using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Refactored.Controllers.Switch
{
    public class PointPartSwitchController : SwitchController
    {
        private readonly PointPart _pointPart;
        private CancellationTokenSource _cts;

        public PointPartSwitchController(PointPart pointPart)
        {
            _pointPart = pointPart;
        }

        protected override async Task OnStartAsync()
        {
            _cts ??= new CancellationTokenSource();

            await _pointPart.ProcessAsync(_cts.Token);
        }

        protected override Task OnStopAsync()
        {
            _cts.Cancel();

            return Task.CompletedTask;
        }
    }
}