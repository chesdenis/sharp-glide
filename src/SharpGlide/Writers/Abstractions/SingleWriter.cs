using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Writers.Interfaces;

namespace SharpGlide.Writers.Abstractions
{
    public class SingleWriter<T> : ISingleWriter<T>
    {
        private readonly Func<T, IRoute, CancellationToken, Task> _singleWriteFunc;
        private readonly Func<T, IRoute, CancellationToken, Task<T>> _singleWriteAndReturnFunc;
       
        public SingleWriter(
            Func<T, IRoute, CancellationToken, Task> singleWriteFunc,
            Func<T, IRoute, CancellationToken, Task<T>> singleWriteAndReturnFunc
        )
        {
            _singleWriteFunc = singleWriteFunc;
            _singleWriteAndReturnFunc = singleWriteAndReturnFunc;
        }
        
        async Task ISingleWriter<T>.Write(T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteFunc(data, route, cancellationToken);

        async Task<T> ISingleWriter<T>.WriteAndReturn(T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteAndReturnFunc(data, route, cancellationToken);
    }  
    
    public class SingleWriter<T, TArg> : ISingleWriter<T,TArg>
    {
        private readonly Func<TArg, T, IRoute, CancellationToken, Task> _singleWriteFunc;
        private readonly Func<TArg, T, IRoute, CancellationToken, Task<T>> _singleWriteAndReturnFunc;
       
        public SingleWriter(
            Func<TArg, T, IRoute, CancellationToken, Task> singleWriteFunc,
            Func<TArg, T, IRoute, CancellationToken, Task<T>> singleWriteAndReturnFunc
        )
        {
            _singleWriteFunc = singleWriteFunc;
            _singleWriteAndReturnFunc = singleWriteAndReturnFunc;
        }
        
        async Task ISingleWriter<T,TArg>.Write(TArg arg, T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteFunc(arg, data, route, cancellationToken);

        async Task<T> ISingleWriter<T,TArg>.WriteAndReturn(TArg arg, T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteAndReturnFunc(arg, data, route, cancellationToken);
    }
}