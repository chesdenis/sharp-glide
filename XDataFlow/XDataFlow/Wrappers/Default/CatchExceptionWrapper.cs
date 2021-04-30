using System;

namespace XDataFlow.Wrappers.Default
{
    public class CatchExceptionWrapper : IWrapper
    {
        private readonly Action _onStart;
        private readonly Action _onException;
        private readonly Action _onFinish;

        public CatchExceptionWrapper(Action onStart, Action onException, Action onFinish)
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
                }
                catch (Exception e)
                {
                    _onException();
                    throw;
                }
                finally
                {
                    _onFinish();
                }
            };
        }
    }
}