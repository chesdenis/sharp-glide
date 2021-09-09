using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using SharpGlide.Extensions;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Providers;
using SharpGlide.Renders;

namespace SharpGlide.Context
{
    public class HeartBeatContext : IHeartBeatContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IConsumeMetrics _consumeMetrics;
        private readonly IGroupContext _groupContext;
        private readonly IMetaDataContext _metaDataContext;

        public HeartBeatContext(
            IDateTimeProvider dateTimeProvider,
            IConsumeMetrics consumeMetrics,
            IGroupContext groupContext,
            IMetaDataContext metaDataContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _consumeMetrics = consumeMetrics;
            _groupContext = groupContext;
            _metaDataContext = metaDataContext;

            LastPublishedAt = _dateTimeProvider.GetNow();
            LastConsumedAt = _dateTimeProvider.GetNow();
        }

        public DateTime LastPublishedAt { get; set; } 
        public DateTime LastConsumedAt { get; set; }
        public int IdleTimeoutMs { get; set; }
        
        public bool Idle => DateTime.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public bool Failed { get; }

        public void UpdateStatus(int indentation = 0)
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name.IndentLeft('-', indentation));
            _metaDataContext.UpsertStatus("Available", _consumeMetrics.GetWaitingToConsumeAmount().ToString());
            _metaDataContext.UpsertStatus("_ETA",_consumeMetrics.GetEstimatedTime().ToString("c"));
            _metaDataContext.UpsertStatus("_Speed, n/sec", _consumeMetrics.GetMessagesPerSecond().ToString());
        }

        public void UpdateStatus(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }

        public List<ExpandoObject> GetStatus(IBasePart startPart)
        {
            var partTree = _groupContext.GetPartTree(startPart).ToList();
            
            foreach (var leaf in partTree)
            {
                leaf.Item2.Context.HeartBeatContext.UpdateStatus(leaf.Item1);
            }
            
            var partsStatusKeys = partTree
                .SelectMany(ss => ss.Item2.Status.Keys)
                .Distinct()
                .ToList();
            
            var dataToPlot = new List<ExpandoObject>();
            
            foreach (var child in partTree)
            {
                var partStatus = new ExpandoObject();
                var partStatusAsDict = (IDictionary<string, object>) partStatus;
            
                foreach (var partsStatusKey in partsStatusKeys)
                {
                    partStatusAsDict[partsStatusKey] =
                        child.Item2.Status.ContainsKey(partsStatusKey) ? child.Item2.Status[partsStatusKey] : string.Empty;
                }
            
                dataToPlot.Add(partStatus);
            }
            
            return dataToPlot;
        }

        public string GetStatusTable(IBasePart startPart)
        {
            var stringBuilder = new StringBuilder();

            var statusInfo = startPart.Context.HeartBeatContext.GetStatus(startPart);

            if (!statusInfo.Any()) return string.Empty;

            stringBuilder.AppendLine(startPart.Name);
            var statusTable = ConsoleTable.FromDynamic(statusInfo);

            stringBuilder.AppendLine(statusTable.ToString());
             
            return stringBuilder.ToString();
        }
    }
}