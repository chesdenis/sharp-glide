using System;
using System.Collections.Generic;
using System.Dynamic;
using XDataFlow.Providers;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Publish;

namespace XDataFlow.Refactored.Controllers.Metric
{
    public class MetricController<TConsumeData, TPublishData> : IMetricController<TConsumeData, TPublishData>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        
        private readonly IConsumeController<TConsumeData> _consumeController;
        private readonly IPublishController<TConsumeData, TPublishData> _publishController;
        private readonly IGroupController _groupController;
        private readonly IMetaDataController _metaDataController;

        public MetricController(
            IDateTimeProvider dateTimeProvider,
            IConsumeController<TConsumeData> consumeController,
            IPublishController<TConsumeData, TPublishData> publishController,
            IGroupController groupController,
            IMetaDataController metaDataController)
        {
            _dateTimeProvider = dateTimeProvider;
            _consumeController = consumeController;
            _publishController = publishController;
            _groupController = groupController;
            _metaDataController = metaDataController;

            LastPublishedAt = _dateTimeProvider.GetNow();
            LastConsumedAt = _dateTimeProvider.GetNow();
        }

        public DateTime LastPublishedAt { get; set; } 
        public DateTime LastConsumedAt { get; set; }
        public int IdleTimeoutMs { get; set; }
        
        public bool Idle => DateTime.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public void PrintStatusInfo()
        {
            _metaDataController.UpsertStatus("Name", _metaDataController.Name);
            _metaDataController.UpsertStatus("Available", _consumeController.GetWaitingToConsumeAmount().ToString());
            _metaDataController.UpsertStatus("ETA",_consumeController.GetEstimatedTime().ToString("c"));
            _metaDataController.UpsertStatus("Speed, n/sec", _consumeController.GetMessagesPerSecond().ToString());
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