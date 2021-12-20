using System;
using System.Collections.Generic;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IDashboard :
        IConsumeRouteConfigure<IXConsumeRoute>,
        IPublishRouteConfigure<IXPublishRoute>,
        ITakenParts
    {
    }

    public class Dashboard : IDashboard
    {
        private const string DefaultQueueName = "Default";
        private const string DefaultRoutingKey = "#";
        private const string DefaultTopic = "#";

        private readonly List<RouteLink> _routesLinks = new List<RouteLink>();

        public IConsumeRouteConfigure<IXConsumeRoute> ConsumeFrom(string queueName, string routingKey)
        {
            _routesLinks.Add(new RouteLink
            {
                ConsumeRoute = new XConsumeRoute
                {
                    RoutingKey = routingKey,
                    Queue = queueName
                },
                PublishRoute = null
            });

            return this;
        }

        public IPublishRouteConfigure<IXPublishRoute> PublishTo(string topic, string routingKey)
        {
            _routesLinks.Add(new RouteLink
            {
                ConsumeRoute = null,
                PublishRoute = new XPublishRoute
                {
                    Topic = topic,
                    RoutingKey = routingKey
                }
            });

            return this;
        }

        public IPublishRouteConfigure<IXPublishRoute> PublishTo(string topic) => PublishTo(topic, DefaultRoutingKey);
        public IPublishRouteConfigure<IXPublishRoute> PublishTo() => PublishTo(DefaultTopic, DefaultRoutingKey);

        public IConsumeRouteConfigure<IXConsumeRoute> ConsumeFrom() => ConsumeFrom(DefaultQueueName, DefaultRoutingKey);

        public IConsumeRouteConfigure<IXConsumeRoute> ConsumeFrom(string queueName) =>
            ConsumeFrom(queueName, DefaultRoutingKey);

        public static IDashboard Create() => new Dashboard();
        public string TakenParts { get; set; }
    }


    public static class RoutesStoreExtensions
    {
        public static IDashboard TakeParts(this IDashboard dashboard, params int[] parts)
        {
            return dashboard;
        }

        public static IDashboard TakePart(this IDashboard dashboard, int part)
        {
            return dashboard;
        }
    }

    public static class TakenPartsExtensions
    {
        public static IDashboard FlowFromSelf(this ITakenParts takenParts)
        {
            return (IDashboard)takenParts;
        }

        public static IDashboard FlowTo(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts)
        {
            return (IDashboard)takenParts;
        }

        public static IDashboard FlowTo(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts, string routingKey)
        {
            return (IDashboard)takenParts;
        }

        public static IDashboard FlowTo(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts, string topic, string routingKey)
        {
            var dashboard = (IDashboard)takenParts;
            
            var partsToConnect = connectedParts(dashboard);

            dashboard.PublishTo(topic, routingKey);
            dashboard.ConsumeFrom(takenParts.TakenParts);
            
            return dashboard;
        }
        
        public static IDashboard FlowFrom(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts)
        {
            return (IDashboard)takenParts;
        }

        public static IDashboard FlowFrom(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts, string queue)
        {
            return (IDashboard)takenParts;
        }

        public static IDashboard FlowFrom(this ITakenParts takenParts,
            Func<IDashboard, ITakenParts> connectedParts, string queue, string routingKey)
        {
            return (IDashboard)takenParts;
        }

        public static ITakenParts Scale(this ITakenParts takenParts,
            int scaleAmount)
        {
            return (IDashboard)takenParts;
        }


        public static ITakenParts Start(this ITakenParts takenParts)
        {
            return (IDashboard)takenParts;
        }

        public static ITakenParts Stop(this ITakenParts takenParts)
        {
            return (IDashboard)takenParts;
        }

        public static ITakenParts Restart(this ITakenParts takenParts)
        {
            return (IDashboard)takenParts;
        }
    }

    public interface ITakenParts
    {
        string TakenParts { get; set; }
    }


    public interface IConsumeRouteConfigure<TRoute> where TRoute : IXConsumeRoute
    {
        IConsumeRouteConfigure<TRoute> ConsumeFrom(string queueName, string routingKey);
        IConsumeRouteConfigure<TRoute> ConsumeFrom(string queueName);
        IConsumeRouteConfigure<TRoute> ConsumeFrom();
    }

    public interface IPublishRouteConfigure<TRoute> where TRoute : IXPublishRoute
    {
        IPublishRouteConfigure<TRoute> PublishTo(string topic, string routingKey);
        IPublishRouteConfigure<TRoute> PublishTo(string topic);
        IPublishRouteConfigure<TRoute> PublishTo();
    }
}