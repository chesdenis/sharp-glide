namespace XDataFlow.Context
{
    public class VectorPartContext<TConsumeData, TPublishData> : PointPartContext
    {
        public IPublishContext<TPublishData> PublishContext { get; }
        public IConsumeContext<TConsumeData> ConsumeContext { get; }

        public VectorPartContext(
            IMetaDataContext metaDataContext, 
            IGroupContext groupContext, 
            IHeartBeatContext heartBeatContext,
            IConsumeMetrics consumeMetrics, 
            ISwitchContext switchContext,
            ISettingsContext settingsContext,
            IConsumeContext<TConsumeData> consumeContext,
            IPublishContext<TPublishData> publishContext) 
            : base(metaDataContext, groupContext, heartBeatContext, switchContext, settingsContext)
        {
            PublishContext = publishContext;
            ConsumeContext = consumeContext;
        }
    }
}