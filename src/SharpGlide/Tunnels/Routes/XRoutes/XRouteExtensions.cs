using System;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public static class XRouteExtensions
    {
        // public static RoutesStore SetupFlow()
        // {
        //     return new RoutesStore();
        // }
        //
        // public static RoutesStore ConsumeFrom(this RoutesStore store)
        // {
        //     
        // }
        //
        // public static RoutesStore FromSelf(this XConsumeRoute consumeRoute, string queueName)
        // {
        //     if (string.IsNullOrWhiteSpace(queueName))
        //     {
        //         return default;
        //     }
        //
        //     var rv = consumeRoute;
        //
        //     rv.Queue = queueName;
        //
        //     return rv;
        // }
        //
        // public static XPublishRoute FlowFrom(this XConsumeRoute consumeRoute, string topic, string routingKey)
        // {
        //     var publishRoute = new XPublishRoute
        //     {
        //         Topic = topic,
        //         RoutingKey = routingKey
        //     };
        //
        //     return publishRoute;
        // }
        //
        // public static XPublishRoute FlowFrom(this XConsumeRoute consumeRoute, string topic)
        //     => FlowFrom(consumeRoute, topic, default(XPublishRoute).RoutingKey);
        //
        // public static XPublishRoute FlowFrom(this XConsumeRoute consumeRoute)
        //     => FlowFrom(consumeRoute, default(XPublishRoute).Topic, default(XPublishRoute).RoutingKey);
        //
        // public static XConsumeRoute FlowTo(this XPublishRoute publishRoute, string routingKey, string queueName)
        // {
        //     return new XConsumeRoute
        //     {
        //         ConnectedRoute = publishRoute,
        //         RoutingKey = routingKey,
        //         Queue = queueName
        //     };
        // }
        //
        // public static XConsumeRoute FlowTo(this XPublishRoute publishRoute, string routingKey) =>
        //     FlowTo(publishRoute, routingKey, default(XConsumeRoute).Queue);
        //
        // public static XConsumeRoute FlowTo(this XPublishRoute publishRoute) => FlowTo(publishRoute,
        //     default(XConsumeRoute).RoutingKey, default(XConsumeRoute).Queue);
    }
}