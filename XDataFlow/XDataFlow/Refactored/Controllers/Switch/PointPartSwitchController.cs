using System.Threading;

namespace XDataFlow.Refactored
{
    public class PointPartSwitchController : SwitchController
    {
        private readonly PointPart _pointPart;
        private CancellationTokenSource _cts;

        public PointPartSwitchController(PointPart pointPart)
        {
            _pointPart = pointPart;
        }
        public override void OnStart()
        {
            _cts ??= new CancellationTokenSource();
            
            _pointPart.Process(_cts.Token);
        }

        public override void OnStop()
        {
            _cts.Cancel();
        }
    }
}