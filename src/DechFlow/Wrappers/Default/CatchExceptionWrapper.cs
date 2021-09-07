using System;

namespace DechFlow.Wrappers.Default
{
    public class CatchExceptionWrapper : IWrapper
    {
        private readonly Action _onStart;
        private readonly Action<Exception> _onException;
        private readonly Action _onFinish;

        public CatchExceptionWrapper(Action onStart, Action<Exception> onException, Action onFinish)
        {
            _onStart = onStart;
            _onException = onException;
            _onFinish = onFinish;
        }
        
        public Action Wrap(Action actionToWrap)
        {
            return () =>
            {
                try
                {
                    _onStart();
                    
                    actionToWrap();
                    
                    _onFinish();
                }
                catch (Exception e)
                {
                    _onException(e);
                    throw;
                }
            };
        }
    }
}