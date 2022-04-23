using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpGlide.Tunnels.Readers
{
    public interface IDirectReaderAsync<T> : IReader<T>
    {
        Task<T> Read();

        Task<IEnumerable<T>> ReadRange();
    }
}