using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.Readers.Abstractions
{
    public class SingleReader<T, TRequest> : ISingleReader<T, TRequest>
    {
        private readonly Func<CancellationToken, TRequest, Task<T>> _singleReadFunc;

        public SingleReader( Func<CancellationToken, TRequest, Task<T>> singleReadFunc)
        {
            _singleReadFunc = singleReadFunc;
        }

        public async Task<T> ReadSingleAsync(CancellationToken cancellationToken, TRequest request) => await _singleReadFunc(cancellationToken, request);

    }

    public class SingleReader<T> : ISingleReader<T>
    {
        private readonly Func<CancellationToken, Task<T>> _singleReadFunc;

        public SingleReader(Func<CancellationToken, Task<T>> singleReadFunc)
        {
            _singleReadFunc = singleReadFunc;
        }

        public async Task<T> ReadSingleAsync(CancellationToken cancellationToken)
            => await _singleReadFunc(cancellationToken);
    }
}