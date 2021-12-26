using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public static class DashboardFlowExtensions
    {
        public static IDashboard FlowFromSelf<TConsumeData, TPublishData>(
            this IDashboardPartsSelection<TConsumeData, TPublishData> partSelection)
        {
            foreach (var vectorPart in partSelection.Selection)
            {
                var prefix = vectorPart.GetPrefix();

                partSelection.Dashboard.ConsumeFrom(
                    $"{prefix}->[selfTopic]",
                    $"{prefix}->[selfQueue]",
                    $"#", vectorPart);
            }

            return partSelection.Dashboard;
        }


        public static IDashboard FlowTo<TConsumeData, TPublishData>(
            this IDashboardPartsSelection<TConsumeData, TPublishData> partSelection,
            params VectorPart<TConsumeData, TPublishData>[] targetParts)
        {
            foreach (var source in partSelection.Selection)
            {
                foreach (var target in targetParts)
                {
                    ConfigureFlowTo(partSelection.Dashboard, source, target);
                }
            }

            return partSelection.Dashboard;
        }

        private static void ConfigureFlowTo<TSConsumeData, TSPublishData, TTConsumeData, TTPublishData>(
            IDashboard dashboard,
            VectorPart<TSConsumeData, TSPublishData> source,
            VectorPart<TTConsumeData, TTPublishData> target,
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