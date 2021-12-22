using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public static class DashboardExtensions
    {
        public static (IEnumerable<VectorPart<TConsumeData, TPublishData>>, IDashboard) SelectParts<TConsumeData, TPublishData>(this IDashboard dashboard, 
            params VectorPart<TConsumeData, TPublishData>[] parts)
        {
            return (parts, dashboard);
        }

        public static IDashboard FlowFromSelf<TConsumeData, TPublishData>(this (IEnumerable<VectorPart<TConsumeData, TPublishData>> selectedParts, IDashboard dashboard) selection)
        {
            foreach (var vectorPart in selection.selectedParts)
            {
                var prefix = vectorPart.GetPrefix();

                selection.dashboard.ConsumeFrom(
                    $"{prefix}->[selfTopic]",
                    $"{prefix}->[selfQueue]",
                    $"#", vectorPart);
            }

            return selection.dashboard;
        }


        public static IDashboard FlowTo<TConsumeData, TPublishData>(this (IEnumerable<VectorPart<TConsumeData, TPublishData>> selectedParts, IDashboard dashboard) selection,
            params VectorPart<TConsumeData, TPublishData>[] targetParts)
        {
            foreach (var source in selection.selectedParts)
            {
                foreach (var target in targetParts)
                {
                    ConfigureFlowTo(selection.dashboard, source, target);
                }
            }

            return selection.dashboard;
        }

        private static void ConfigureFlowTo<
            TSConsumeData, TSPublishData,
            TTConsumeData, TTPublishData
        >(IDashboard dashboard,
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