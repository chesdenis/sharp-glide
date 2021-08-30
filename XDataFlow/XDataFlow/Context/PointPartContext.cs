namespace XDataFlow.Context
{
    public class PointPartContext : IPartContext
    {
        public IMetaDataContext MetaDataContext { get; }
        public IGroupContext GroupContext { get; }
        public IHeartBeatContext HeartBeatContext { get; }
        public ISwitchContext SwitchContext { get; }

        public PointPartContext(
            IMetaDataContext metaDataContext, 
            IGroupContext groupContext, 
            IHeartBeatContext heartBeatContext, 
            ISwitchContext switchContext)
        {
            MetaDataContext = metaDataContext;
            GroupContext = groupContext;
            HeartBeatContext = heartBeatContext;
             
            SwitchContext = switchContext;
        }
 
    }
}