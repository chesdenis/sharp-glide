using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers
{
    public interface IFilteredReader<T>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
    
    public interface IFilteredReader<T, in TArg>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken,
            TArg request,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
}