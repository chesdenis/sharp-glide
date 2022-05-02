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

        public async Task WriteSingle(T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteFunc(data, route, cancellationToken);

        public async Task<T> WriteAndReturnSingle(T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteAndReturnFunc(data, route, cancellationToken);
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

        public async Task WriteSingle(TArg arg, T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteFunc(arg, data, route, cancellationToken);

        public async Task<T> WriteAndReturnSingle(TArg arg, T data, IRoute route, CancellationToken cancellationToken) => await _singleWriteAndReturnFunc(arg, data, route, cancellationToken);
    }
}