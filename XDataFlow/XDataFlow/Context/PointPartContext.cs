namespace XDataFlow.Context
{
    public class PointPartContext : IPartContext
    {
        public IMetaDataContext MetaDataContext { get; }
        public IGroupContext GroupContext { get; }
        public IHeartBeatContext HeartBeatContext { get; }
        public IConsumeMetrics ConsumeMetrics { get; }
        public ISwitchContext SwitchContext { get; }

        public PointPartContext(
            IMetaDataContext metaDataContext, 
            IGroupContext groupContext, 
            IHeartBeatContext heartBeatContext, 
            IConsumeMetrics consumeMetrics, 
            ISwitchContext switchContext)
        {
            MetaDataContext = metaDataContext;
            GroupContext = groupContext;
            HeartBeatContext = heartBeatContext;
            ConsumeMetrics = consumeMetrics;
            SwitchContext = switchContext;
        }
 
    }
}