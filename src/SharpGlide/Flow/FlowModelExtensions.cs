using System;
using System.Linq;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Write.Interfaces;
using SharpGlide.Writers;

namespace SharpGlide.Flow
{
    public static class FlowModelExtensions
    {
        public static bool IsInterfaceOf(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(w => w == interfaceType);
        }

        public static FlowModel AddTunnel<T>(this FlowModel flowModel,
            Func<FlowModel, T> configure,
            string name = null) where T : ITunnel
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;
            var tunnel = configure(flowModel);
            flowModel.Tunnels.Add(name, tunnel);

            return flowModel;
        }

        public static FlowModel AddTunnel(this FlowModel flowModel, ITunnel tunnel, string name = null)
        {
            name = string.IsNullOrWhiteSpace(name) ? tunnel.GetType().Name : name;
            flowModel.Tunnels.Add(name, tunnel);
            return flowModel;
        }

        public static FlowModel AddPart<T>(this FlowModel flowModel, Func<FlowModel, T> configure, string name = null)
            where T : IBasePart
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;

            var part = configure(flowModel);
            flowModel.Parts.Add(name, part);

            return flowModel;
        }

        public static IReader<TData> BuildReader<TData>(this FlowModel flowModel,
            IReadTunnel<TData> tunnel)
        {
            return new Reader<TData>(
                tunnel.ReadExpr.Compile(),
                tunnel.ReadAllExpr.Compile(),
                tunnel.ReadPagedExpr.Compile(),
                tunnel.ReadSpecificExpr.Compile());
        }

        public static IReaderWithArg<TData, TRequest> BuildReader<TData, TRequest>(this FlowModel flowModel,
            IReadWithArgTunnel<TData, TRequest> withArgTunnel)
        {
            return new ReaderWithArg<TData, TRequest>(
                withArgTunnel.ReadExpr.Compile(),
                withArgTunnel.ReadAllExpr.Compile(),
                withArgTunnel.ReadPagedExpr.Compile(),
                withArgTunnel.ReadSpecificExpr.Compile()
            );
        }

        public static IWriter<TData> BuildWriter<TData>(this FlowModel flowModel,
            IWriteTunnel<TData> tunnel)
        {
            return new Writer<TData>(
                tunnel.WriteSingleExpr.Compile(),
                tunnel.WriteAndReturnSingleExpr.Compile(),
                tunnel.WriteRangeExpr.Compile(),
                tunnel.WriteAndReturnRangeExpr.Compile());
        }

        public static IWalker<TData> BuildWalker<TData>(this FlowModel flowModel,
            IWalkTunnel<TData> tunnel)
        {
            return new Walker<TData>(
                tunnel.WalkExpr.Compile(),
                tunnel.WalkAsyncExpr.Compile(),
                tunnel.WalkPagedExpr.Compile(),
                tunnel.WalkPagedAsyncExpr.Compile()
            );
        }

        public static IWalkerWithArg<TData, TRequest> BuildWalker<TData, TRequest>(this FlowModel flowModel,
            IWalkWithArgTunnel<TData, TRequest> tunnel)
        {
            return new WalkerWithArg<TData, TRequest>(
                tunnel.WalkExpr.Compile(),
                tunnel.WalkPagedExpr.Compile()
            );
        }
    }
}