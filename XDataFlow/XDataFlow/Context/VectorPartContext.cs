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
            IConsumeContext<TConsumeData> consumeContext,
            IPublishContext<TPublishData> publishContext) 
            : base(metaDataContext, groupContext, heartBeatContext, switchContext)
        {
            PublishContext = publishContext;
            ConsumeContext = consumeContext;
        }
    }
}