using System;
using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public class TryCatchStart : Start
    {
        private readonly Action _onStart;
        private readonly Action<Exception> _onException;
        private readonly Action _onFinish;
        
        public TryCatchStart(Action onStart, Action<Exception> onException, Action onFinish)
        {
            _onStart = onStart;
            _onException = onException;
            _onFinish = onFinish;
        }

        public override async Task ExecuteAsync(ISwitchController switchController)
        {
            try
            {
                _onStart();

                await base.ExecuteAsync(switchController);

                _onFinish();
            }
            catch (Exception e)
            {
                _onException(e);
                throw;
            }
        }
    }
}