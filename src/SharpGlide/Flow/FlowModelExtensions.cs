using System;
using System.Linq;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Readers.Abstractions;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Write.Abstractions;
using SharpGlide.Tunnels.Write.Interfaces;
using SharpGlide.Writers;
using SharpGlide.Writers.Abstractions;

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

        public static ISingleReader<TData> BuildReader<TData>(this FlowModel flowModel,
            ISingleReadTunnel<TData> tunnel)
        {
            return new SingleReader<TData>(
                tunnel.ReadSingleExpr.Compile()
            );
        }

        public static ISingleReader<TData, TArg> BuildReader<TData, TArg>(this FlowModel flowModel,
            ISingleReadTunnel<TData, TArg> tunnel)
        {
            return new SingleReader<TData, TArg>(
                tunnel.ReadSingleExpr.Compile()
            );
        }

        public static Reader<TData> BuildReader<TData>(this FlowModel flowModel,
            ReadTunnel<TData> tunnel)
        {
            return new Reader<TData>(
                ((ISingleReadTunnel<TData>)tunnel).ReadSingleExpr.Compile(),
                ((ICollectionReadTunnel<TData>)tunnel).ReadCollectionExpr.Compile(),
                ((IPagedReadTunnel<TData>)tunnel).ReadPagedExpr.Compile(),
                ((IFilteredReadTunnel<TData>)tunnel).ReadFilteredExpr.Compile());
        }

        public static Reader<TData, TArg> BuildReader<TData, TArg>(this FlowModel flowModel,
            ReadTunnel<TData, TArg> tunnel)
        {
            return new Reader<TData, TArg>(
                ((ISingleReadTunnel<TData, TArg>)tunnel).ReadSingleExpr.Compile(),
                ((ICollectionReadTunnel<TData, TArg>)tunnel).ReadCollectionExpr.Compile(),
                ((IPagedReadTunnel<TData, TArg>)tunnel).ReadPagedExpr.Compile(),
                ((IFilteredReadTunnel<TData, TArg>)tunnel).ReadFilteredExpr.Compile());
        }

        public static SingleWriter<TData> BuildSingleWriter<TData>(this FlowModel flowModel,
            SingleWriteTunnel<TData> tunnel)
        {
            return new SingleWriter<TData>(
                ((ISingleWriteTunnel<TData>)tunnel).WriteSingleExpr.Compile(),
                ((ISingleWriteTunnel<TData>)tunnel).WriteAndReturnExpr.Compile());
        }
        
        public static CollectionWriter<TData> BuildCollectionWriter<TData>(this FlowModel flowModel,
            CollectionWriteTunnel<TData> tunnel)
        {
            return new CollectionWriter<TData>(
                ((ICollectionWriteTunnel<TData>)tunnel).WriteCollectionExpr.Compile(),
                ((ICollectionWriteTunnel<TData>)tunnel).WriteAndReturnCollectionExpr.Compile());
        }

        public static Walker<TData> BuildWalker<TData>(this FlowModel flowModel,
            WalkTunnel<TData> tunnel)
        {
            return new Walker<TData>(
                ((ISingleWalkTunnel<TData>)tunnel).WalkSingleExpr.Compile(),
                ((ISingleAsyncWalkTunnel<TData>)tunnel).WalkSingleAsyncExpr.Compile(),
                ((IPagedWalkTunnel<TData>)tunnel).WalkPagedExpr.Compile(),
                ((IPagedAsyncWalkTunnel<TData>)tunnel).WalkPagedAsyncExpr.Compile()
            );
        }

        public static Walker<TData, TArg> BuildWalker<TData, TArg>(this FlowModel flowModel,
            WalkTunnel<TData, TArg> tunnel)
        {
            return new Walker<TData, TArg>(
                ((ISingleWalkTunnel<TData, TArg>)tunnel).WalkSingleExpr.Compile(),
                ((ISingleAsyncWalkTunnel<TData, TArg>)tunnel).WalkSingleAsyncExpr.Compile(),
                ((IPagedWalkTunnel<TData, TArg>)tunnel).WalkPagedExpr.Compile(),
                ((IPagedAsyncWalkTunnel<TData, TArg>)tunnel).WalkPagedAsyncExpr.Compile()
            );
        }
    }
}