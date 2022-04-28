using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class WalkTunnel<T> : IWalkTunnel<T>
    {
        public bool CanExecute { get; set; }


        public Expression<Func<CancellationToken,Action<T>, Task>> WalkExpr => 
            (cancellationToken, callBack) => WalkImpl(cancellationToken, callBack);
        protected abstract Task WalkImpl(CancellationToken cancellationToken, Action<T> callBack);

        public Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr =>
            (cancellationToken, pageInfo, callback) => WalkPagedImpl(cancellationToken, pageInfo, callback);

        protected abstract Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback);
    }
}