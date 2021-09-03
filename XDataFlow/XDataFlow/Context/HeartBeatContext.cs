using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using XDataFlow.Providers;

namespace XDataFlow.Context
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
            IGroupContext groupContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _consumeMetrics = consumeMetrics;
            _groupContext = groupContext;

            LastPublishedAt = _dateTimeProvider.GetNow();
            LastConsumedAt = _dateTimeProvider.GetNow();
        }

        public DateTime LastPublishedAt { get; set; } 
        public DateTime LastConsumedAt { get; set; }
        public int IdleTimeoutMs { get; set; }
        
        public bool Idle => DateTime.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public bool Failed { get; }

        public void UpdateStatus()
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name);
            _metaDataContext.UpsertStatus("Available", _consumeMetrics.GetWaitingToConsumeAmount().ToString());
            _metaDataContext.UpsertStatus("ETA",_consumeMetrics.GetEstimatedTime().ToString("c"));
            _metaDataContext.UpsertStatus("Speed, n/sec", _consumeMetrics.GetMessagesPerSecond().ToString());
        }

        public void UpdateStatus(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }
        
        public List<ExpandoObject> GetStatus()
        {
            var children = _groupContext.GetChildrenTree()
                .Select(s=>s.Item2).ToList();
            
            foreach (var child in children)
            {
                child.Context.HeartBeatContext.UpdateStatus();
            }
            
            var partsStatusKeys = children
                .SelectMany(ss => ss.Status.Keys)
                .Distinct()
                .OrderBy(o => o)
                .ToList();
            
            var dataToPlot = new List<ExpandoObject>();
            
            foreach (var child in children)
            {

                var partStatus = new ExpandoObject();
                var partStatusAsDict = (IDictionary<string, object>) partStatus;
            
                foreach (var partsStatusKey in partsStatusKeys)
                {
                    partStatusAsDict[partsStatusKey] =
                        child.Status.ContainsKey(partsStatusKey) ? child.Status[partsStatusKey] : string.Empty;
                }
            
                dataToPlot.Add(partStatus);
            }
            
            return dataToPlot;
        }
    }
}