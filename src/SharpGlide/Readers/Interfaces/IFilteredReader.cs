using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers.Interfaces
{
    public interface IFilteredReader<T>
    {
        Task<IEnumerable<T>> ReadFilteredAsync(CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
    
    public interface IFilteredReader<T, in TArg>
    {
        Task<IEnumerable<T>> ReadFilteredAsync(CancellationToken cancellationToken,
            TArg request,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
}