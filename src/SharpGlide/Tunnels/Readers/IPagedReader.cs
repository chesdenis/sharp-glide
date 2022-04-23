using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IPagedReader<T> : IReader<T>
    {
        Expression<Action<Action<IEnumerable<T>, PageInfo>, PageInfo>> ReadPageExpr { get; }
    }
}