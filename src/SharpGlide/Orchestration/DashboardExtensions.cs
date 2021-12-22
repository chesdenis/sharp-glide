using System;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public static class DashboardExtensions
    {
        public static IDashboard TakeParts(this IDashboard dashboard, params IBasePart[] parts)
        {
            dashboard.Selection.Clear();

            foreach (var part in parts)
            {
                dashboard.Selection.Add(part);
            }

            return dashboard;
        }

        public static IDashboard FlowFromSelf(this IDashboard dashboard)
        {
            foreach (var part in dashboard.EnumerateSelection())
            {
                var prefix = part.GetPrefix();

                dashboard.ConsumeFrom(
                    $"{prefix}->[selfTopic]",
                    $"{prefix}->[selfQueue]",
                    $"#", part);
            }

            return dashboard;
        }

        public static IDashboard FlowTo(this IDashboard dashboard, params IBasePart[] targetParts)
        {
            foreach (var source in dashboard.EnumerateSelection())
            foreach (var target in targetParts)
                ConfigureFlowTo(dashboard, source, target);

            return dashboard;
        }

        private static void ConfigureFlowTo(IDashboard dashboard,
            IBasePart source,
            IBasePart target,
            string sourceTopic = "",
            string sourceRoutingKey = "",
            string sourceQueue = "",
            string targetTopic = "",
            string targetQueue = "",
            string targetRoutingKey = "")
        {
            var sourcePrefix = source.GetPrefix();
            var targetPrefix = target.GetPrefix();

            dashboard.PublishTo(
                string.IsNullOrWhiteSpace(sourceTopic) ? $"[{sourcePrefix}]" : sourceTopic,
                string.IsNullOrWhiteSpace(sourceRoutingKey) ? "#" : sourceRoutingKey,
                string.IsNullOrWhiteSpace(sourceQueue) ? $"[{sourcePrefix}]->[{targetPrefix}]" : sourceQueue, source);

            dashboard.ConsumeFrom(
                string.IsNullOrWhiteSpace(targetTopic) ? $"[{sourcePrefix}]" : targetTopic,
                string.IsNullOrWhiteSpace(targetQueue) ? $"[{sourcePrefix}]->[{targetPrefix}]" : targetQueue,
                string.IsNullOrWhiteSpace(targetRoutingKey) ? "#" : targetRoutingKey,
                target);
        }

        public static IDashboard FlowTo(this IDashboard dashboard,
            Func<IDashboard, IDashboardSelection> partSelect, string routingKey)
        {
            return dashboard;
        }


        public static IDashboard FlowTo(this IDashboard dashboard,
            Func<IDashboard, IDashboardSelection> partSelect, string topic, string routingKey)
        {
            return dashboard;
        }

        public static IDashboard FlowFrom(this IDashboard dashboard,
            Func<IDashboard, IDashboardSelection> partSelect)
        {
            return dashboard;
        }

        public static IDashboard FlowFrom(this IDashboard dashboard,
            Func<IDashboard, IDashboardSelection> connectedParts, string queue)
        {
            return dashboard;
        }

        public static IDashboard FlowFrom(this IDashboard dashboard,
            Func<IDashboard, IDashboardSelection> connectedParts, string queue, string routingKey)
        {
            return dashboard;
        }

        public static IDashboard Scale(this IDashboard dashboard, int scaleAmount)
        {
            return dashboard;
        }

        public static IDashboard Start(this IDashboard dashboard)
        {
            return dashboard;
        }

        public static IDashboard Stop(this IDashboard dashboard)
        {
            return dashboard;
        }

        public static IDashboard Restart(this IDashboard dashboard)
        {
            return dashboard;
        }

        private static string GetPrefix(this IBasePart part)
        {
            if (string.IsNullOrWhiteSpace(part.Name))
            {
                return $"Unnamed {part.GetType().Name}";
            }

            return part.Name;
        }
    }
}