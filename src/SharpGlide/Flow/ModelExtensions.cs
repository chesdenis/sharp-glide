using System;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Proxy;
using SharpGlide.Tunnels.Writers.Interfaces;
using SharpGlide.Tunnels.Writers.Proxy;

namespace SharpGlide.Flow
{
    public static class ModelExtensions
    {
        public static Model AddTunnel<T>(this Model model, 
            Func<Model, T> configure, 
            string name = null) where T: ITunnel
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;
            var tunnel = configure(model);
            model.Tunnels.Add(name, tunnel);

            return model;
        }

        public static IDirectReaderProxy<TData> GetDirectReaderProxy<TData>(this Model model,
            IDirectReader<TData> tunnel)
        {
            return new DirectReaderProxy<TData>(
                tunnel.ReadExpr.Compile(),
                tunnel.ReadAllExpr.Compile(),
                tunnel.ReadPagedExpr.Compile(),
                tunnel.ReadSpecificExpr.Compile());
        }
        
        public static IDirectWriterProxy<TData> GetDirectWriterProxy<TData>(this Model model,
            IDirectWriter<TData> tunnel)
        {
            return new DirectWriterProxy<TData>(
                tunnel.WriteSingleExpr.Compile(),
                tunnel.WriteAndReturnSingleExpr.Compile(),
                tunnel.WriteRangeExpr.Compile(),
                tunnel.WriteAndReturnRangeExpr.Compile());
        }

        public static IDirectWalkerProxy<TData> GetDirectWalkerProxy<TData>(this Model model,
            IDirectWalker<TData> tunnel)
        {
            return new DirectWalkerProxy<TData>(
                tunnel.WalkExpr.Compile(),
                tunnel.WalkPagedExpr.Compile()
                );
        }
    }
}