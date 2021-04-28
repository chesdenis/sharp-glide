namespace XDataFlow.Tunnels
{
    public interface IPublishTunnel
    {
        bool CanPublishThis(byte[] data);
        
        void Publish(byte[] data);
    }
}