using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class SingleReadTunnel<T> : ISingleReadTunnel<T>
    {
        public bool CanExecute { get; set; }
        
        Expression<Func<CancellationToken, Task<T>>> ISingleReadTunnel<T>.ReadExpr =>
            (cancellationToken)
                => SingleReadImpl(cancellationToken);
        
        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken);
    }
    
    public abstract class SingleReadTunnel<T, TArg> : ISingleReadTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }
        
        Expression<Func<CancellationToken,TArg, Task<T>>> ISingleReadTunnel<T, TArg>.ReadExpr =>
            (cancellationToken, arg)
                => SingleReadImpl(cancellationToken, arg);
        
        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken, TArg arg);
    }
}