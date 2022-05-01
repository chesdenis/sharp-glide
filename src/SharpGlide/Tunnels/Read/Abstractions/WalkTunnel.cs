using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class WalkTunnel<T> :
        ISingleWalkTunnel<T>,
        ISingleAsyncWalkTunnel<T>,
        IPagedWalkTunnel<T>,
        IPagedAsyncWalkTunnel<T>
    {
        public bool CanExecute { get; set; }

        Expression<Func<CancellationToken, Action<T>, Task>> ISingleWalkTunnel<T>.WalkExpr
            => (cancellationToken, callback) => SingleWalkImpl(cancellationToken, callback);

        Expression<Func<CancellationToken, Func<CancellationToken, T, Task>, Task>> ISingleAsyncWalkTunnel<T>.WalkExpr
            => (cancellationToken, callback) => SingleAsyncWalkImpl(cancellationToken, callback);

        Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> IPagedWalkTunnel<T>.WalkExpr
            => (cancellationToken, pageInfo, callback) => PagedWalkImpl(cancellationToken, pageInfo, callback);

        Expression<Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>>
            IPagedAsyncWalkTunnel<T>.WalkExpr
            => (cancellationToken, pageInfo, callback) => PagedAsyncWalkImpl(cancellationToken, pageInfo, callback);

        protected abstract Task SingleWalkImpl(CancellationToken cancellationToken, Action<T> callback);

        protected abstract Task SingleAsyncWalkImpl(CancellationToken cancellationToken,
            Func<CancellationToken, T, Task> callback);

        protected abstract Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<T>> callback);

        protected abstract Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callback);
    }
    
    public abstract class WalkTunnel<T, TArg> :
        ISingleWalkTunnel<T, TArg>,
        ISingleAsyncWalkTunnel<T, TArg>,
        IPagedWalkTunnel<T, TArg>,
        IPagedAsyncWalkTunnel<T, TArg>
    {
         public bool CanExecute { get; set; }
        
         Expression<Func<CancellationToken, TArg, Action<T>, Task>> ISingleWalkTunnel<T, TArg>.WalkExpr 
             => (cancellationToken, request, callback) =>  SingleWalkImpl(cancellationToken, request, callback);

         Expression<Func<CancellationToken, TArg, Func<CancellationToken, T, Task>, Task>> ISingleAsyncWalkTunnel<T, TArg>.WalkExpr 
             => (cancellationToken, request, callback) => SingleAsyncWalkImpl(cancellationToken, request, callback);

         Expression<Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task>> IPagedWalkTunnel<T, TArg>.WalkExpr 
             => (cancellationToken, pageInfo, request, callback) => PagedWalkImpl(cancellationToken, pageInfo,request, callback);

         Expression<Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task>> IPagedAsyncWalkTunnel<T, TArg>.WalkExpr 
             => (cancellationToken, pageInfo, request, callback) => PagedAsyncWalkImpl(cancellationToken, pageInfo, request, callback);

        
        protected abstract Task SingleWalkImpl(CancellationToken cancellationToken, 
            TArg request, Action<T> callback);
        
        protected abstract Task SingleAsyncWalkImpl(CancellationToken cancellationToken, 
            TArg request, Func<CancellationToken, T, Task> callback);
        
        
        protected abstract Task PagedWalkImpl(CancellationToken cancellationToken, 
            PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback);
        
        
        protected abstract Task PagedAsyncWalkImpl(CancellationToken cancellationToken, 
            PageInfo pageInfo, TArg request,  Func<CancellationToken, IEnumerable<T>, Task> callback);
     
    }
}