using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IReadTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Task<T>>> ReadExpr { get; }
        
        Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadAllExpr { get; }
        
        Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> ReadPagedExpr { get; }
        
        Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadSpecificExpr { get; }
    }
}