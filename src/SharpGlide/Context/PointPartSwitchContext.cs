using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context
{
    public class PointPartSwitchContext : SwitchContext
    {
        private readonly PointPart _pointPart;
        private CancellationTokenSource _cts;

        public PointPartSwitchContext(PointPart pointPart) : base(pointPart)
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