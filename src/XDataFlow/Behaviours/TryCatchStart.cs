using System;
using System.Threading.Tasks;
using XDataFlow.Context;

namespace XDataFlow.Behaviours
{
    public class TryCatchStart : Start
    {
        private readonly Action _onStart;
        private readonly Action _onProcessedOk;
        private readonly Action<Exception> _onProcessedWithFailure;
        private readonly Action _onFinalise;
        
        public TryCatchStart(
            Action onStart,
            Action onProcessedOk, 
            Action<Exception> onProcessedWithFailure, 
            Action onFinalise)
        {
            _onStart = onStart;
            _onProcessedOk = onProcessedOk;
            _onProcessedWithFailure = onProcessedWithFailure;
            _onFinalise = onFinalise;
        }

        public override async Task ExecuteAsync(ISwitchContext switchContext)
        {
            try
            {
                _onStart();

                await base.ExecuteAsync(switchContext);

                _onProcessedOk();

            }
            catch (Exception e)
            {
                _onProcessedWithFailure(e);
                throw;
            }
            finally
            {
                _onFinalise();
            }
        }
    }
}