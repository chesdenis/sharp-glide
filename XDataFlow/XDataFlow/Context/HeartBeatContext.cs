using System;
using System.Collections.Generic;
using System.Dynamic;
using XDataFlow.Providers;

namespace XDataFlow.Context
{
    public class HeartBeatContext : IHeartBeatContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IConsumeMetrics _consumeMetrics;
        private readonly IMetaDataContext _metaDataContext;

        public HeartBeatContext(
            IDateTimeProvider dateTimeProvider,
            IConsumeMetrics consumeMetrics)
        {
            _dateTimeProvider = dateTimeProvider;
            _consumeMetrics = consumeMetrics;

            LastPublishedAt = _dateTimeProvider.GetNow();
            LastConsumedAt = _dateTimeProvider.GetNow();
        }

        public DateTime LastPublishedAt { get; set; } 
        public DateTime LastConsumedAt { get; set; }
        public int IdleTimeoutMs { get; set; }
        
        public bool Idle => DateTime.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public bool Failed { get; }

        public void PrintStatusInfo()
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name);
            _metaDataContext.UpsertStatus("Available", _consumeMetrics.GetWaitingToConsumeAmount().ToString());
            _metaDataContext.UpsertStatus("ETA",_consumeMetrics.GetEstimatedTime().ToString("c"));
            _metaDataContext.UpsertStatus("Speed, n/sec", _consumeMetrics.GetMessagesPerSecond().ToString());
        }

        public void PrintStatusInfo(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }
        
        public List<ExpandoObject> GetStatusInfo()
        {
            throw new NotImplementedException();
            // TODO: need to apply recursion here, may be have children in metric controller?
            // foreach (var partsKey in _groupController.Children.Keys)
            // {
            //     var part = _groupController.Children[partsKey];
            //     part.PrintStatusInfo();
            // }
            //
            // var partsStatusKeys = _groupController.Children
            //     .Select(s => s.Value)
            //     .SelectMany(ss => ss.Status.Keys)
            //     .Distinct()
            //     .OrderBy(o => o)
            //     .ToList();
            //
            // var dataToPlot = new List<ExpandoObject>();
            //
            // foreach (var partsKey in _groupController.Children.Keys)
            // {
            //     var part = _groupController.Children[partsKey];
            //
            //     var partStatus = new ExpandoObject();
            //     var partStatusAsDict = (IDictionary<string, object>) partStatus;
            //
            //     foreach (var partsStatusKey in partsStatusKeys)
            //     {
            //         partStatusAsDict[partsStatusKey] =
            //             part.Status.ContainsKey(partsStatusKey) ? part.Status[partsStatusKey] : string.Empty;
            //     }
            //
            //     dataToPlot.Add(partStatus);
            // }
            //
            // return dataToPlot;
        }
    }
}