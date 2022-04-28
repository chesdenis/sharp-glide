using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Writers
{
    public class Writer<T> : IWriter<T>
    {
        private readonly Func<T, IRoute, CancellationToken, Task> _writeSingleFunc;
        private readonly Func<T, IRoute, CancellationToken, Task<T>> _writeAndReturnSingleFunc;
        private readonly Func<IEnumerable<T>, IRoute, CancellationToken, Task> _writeRangeFunc;
        private readonly Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> _writeAndReturnRangeFunc;

        public Writer(
            Func<T, IRoute, CancellationToken, Task> writeSingleFunc,
            Func<T, IRoute, CancellationToken, Task<T>> writeAndReturnSingleFunc,
            Func<IEnumerable<T>, IRoute, CancellationToken, Task> writeRangeFunc,
            Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> writeAndReturnRangeFunc
        )
        {
            _writeSingleFunc = writeSingleFunc;
            _writeAndReturnSingleFunc = writeAndReturnSingleFunc;
            _writeRangeFunc = writeRangeFunc;
            _writeAndReturnRangeFunc = writeAndReturnRangeFunc;
        }

        public async Task WriteSingle(T data, IRoute route, CancellationToken cancellationToken) =>
            await _writeSingleFunc(data, route, cancellationToken);

        public async Task<T> WriteAndReturnSingle(T data, IRoute route, CancellationToken cancellationToken) =>
            await _writeAndReturnSingleFunc(data, route, cancellationToken);
        
        public async Task WriteRange(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken) =>
            await _writeRangeFunc(dataRange, route, cancellationToken); 
        
        public async Task<IEnumerable<T>> WriteAndReturnRange(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken) =>
            await _writeAndReturnRangeFunc(dataRange, route, cancellationToken); 
    }
}