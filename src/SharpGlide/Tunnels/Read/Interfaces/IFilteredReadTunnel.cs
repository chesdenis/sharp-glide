using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IFilteredReadTunnel<T,TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadFilteredExpr { get; }
    }

    public interface IFilteredReadTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadFilteredExpr
        {
            get;
        }
    }
}