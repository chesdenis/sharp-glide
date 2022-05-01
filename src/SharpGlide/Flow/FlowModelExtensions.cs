using System;
using System.Linq;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Write.Abstractions;
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

        public static Reader<TData> BuildReader<TData>(this FlowModel flowModel,
            ReadTunnel<TData> tunnel)
        {
            return new Reader<TData>(
                ((ISingleReadTunnel<TData>)tunnel).ReadExpr.Compile(),
                ((ICollectionReadTunnel<TData>)tunnel).ReadExpr.Compile(),
                ((IPagedReadTunnel<TData>)tunnel).ReadExpr.Compile(),
                ((IFilteredReadTunnel<TData>)tunnel).ReadExpr.Compile());
        }

        public static Reader<TData, TArg> BuildReader<TData, TArg>(this FlowModel flowModel,
            ReadTunnel<TData, TArg> tunnel)
        {
            return new Reader<TData, TArg>(
                ((ISingleReadTunnel<TData, TArg>)tunnel).ReadExpr.Compile(),
                ((ICollectionReadTunnel<TData, TArg>)tunnel).ReadExpr.Compile(),
                ((IPagedReadTunnel<TData, TArg>)tunnel).ReadExpr.Compile(),
                ((IFilteredReadTunnel<TData, TArg>)tunnel).ReadExpr.Compile());
        }

        public static Writer<TData> BuildWriter<TData>(this FlowModel flowModel,
            WriteTunnel<TData> tunnel)
        {
            return new Writer<TData>(
                tunnel.WriteSingleExpr.Compile(),
                tunnel.WriteAndReturnSingleExpr.Compile(),
                tunnel.WriteRangeExpr.Compile(),
                tunnel.WriteAndReturnRangeExpr.Compile());
        }

        public static Walker<TData> BuildWalker<TData>(this FlowModel flowModel,
            WalkTunnel<TData> tunnel)
        {
            return new Walker<TData>(
                ((ISingleWalkTunnel<TData>)tunnel).WalkExpr.Compile(),
                ((ISingleAsyncWalkTunnel<TData>)tunnel).WalkExpr.Compile(),
                ((IPagedWalkTunnel<TData>)tunnel).WalkExpr.Compile(),
                ((IPagedAsyncWalkTunnel<TData>)tunnel).WalkExpr.Compile()
            );
        }

        public static Walker<TData, TArg> BuildWalker<TData, TArg>(this FlowModel flowModel,
            WalkTunnel<TData, TArg> tunnel)
        {
            return new Walker<TData, TArg>(
                ((ISingleWalkTunnel<TData, TArg>)tunnel).WalkExpr.Compile(),
                ((ISingleAsyncWalkTunnel<TData, TArg>)tunnel).WalkExpr.Compile(),
                ((IPagedWalkTunnel<TData, TArg>)tunnel).WalkExpr.Compile(),
                ((IPagedAsyncWalkTunnel<TData, TArg>)tunnel).WalkExpr.Compile()
            );
        }
    }
}