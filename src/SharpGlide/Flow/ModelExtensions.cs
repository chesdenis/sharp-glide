using System;
using System.Linq;
using SharpGlide.Parts;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Proxy;
using SharpGlide.Tunnels.Writers.Interfaces;
using SharpGlide.Tunnels.Writers.Proxy;

namespace SharpGlide.Flow
{
    public static class ModelExtensions
    {
        public static bool IsInterfaceOf(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(w => w == interfaceType);
        }

        public static Model AddTunnel<T>(this Model model,
            Func<Model, T> configure,
            string name = null) where T : ITunnel
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;
            var tunnel = configure(model);
            model.Tunnels.Add(name, tunnel);

            return model;
        }

        public static Model AddTunnel(this Model model, ITunnel tunnel, string name = null)
        {
            name = string.IsNullOrWhiteSpace(name) ? tunnel.GetType().Name : name;
            model.Tunnels.Add(name, tunnel);
            return model;
        }

        public static Model AddPart<T>(this Model model, Func<Model, T> configure, string name = null)
            where T : IBasePart
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;

            var part = configure(model);
            model.Parts.Add(name, part);

            return model;
        }

        public static IDirectReaderProxy<TData> GetProxy<TData>(this Model model,
            IDirectReader<TData> tunnel)
        {
            return new DirectReaderProxy<TData>(
                tunnel.ReadExpr.Compile(),
                tunnel.ReadAllExpr.Compile(),
                tunnel.ReadPagedExpr.Compile(),
                tunnel.ReadSpecificExpr.Compile());
        }

        public static IReaderByRequestProxy<TData, TRequest> GetProxy<TData, TRequest>(this Model model,
            IReaderByRequest<TData, TRequest> tunnel)
        {
            return new ReaderByRequestProxy<TData, TRequest>(
                tunnel.ReadExpr.Compile(),
                tunnel.ReadAllExpr.Compile(),
                tunnel.ReadPagedExpr.Compile(),
                tunnel.ReadSpecificExpr.Compile()
            );
        }

        public static IDirectWriterProxy<TData> GetProxy<TData>(this Model model,
            IDirectWriter<TData> tunnel)
        {
            return new DirectWriterProxy<TData>(
                tunnel.WriteSingleExpr.Compile(),
                tunnel.WriteAndReturnSingleExpr.Compile(),
                tunnel.WriteRangeExpr.Compile(),
                tunnel.WriteAndReturnRangeExpr.Compile());
        }

        public static IDirectWalkerProxy<TData> GetProxy<TData>(this Model model,
            IDirectWalker<TData> tunnel)
        {
            return new DirectWalkerProxy<TData>(
                tunnel.WalkExpr.Compile(),
                tunnel.WalkPagedExpr.Compile()
            );
        }

        public static IWalkerByRequestProxy<TData, TRequest> GetProxy<TData, TRequest>(this Model model,
            IWalkerByRequest<TData, TRequest> tunnel)
        {
            return new WalkerByRequestProxy<TData, TRequest>(
                tunnel.WalkExpr.Compile(),
                tunnel.WalkPagedExpr.Compile()
            );
        }
    }
}