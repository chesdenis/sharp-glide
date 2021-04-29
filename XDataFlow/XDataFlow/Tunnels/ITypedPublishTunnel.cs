namespace XDataFlow.Tunnels
{
    public interface ITypedPublishTunnel<in T>
    {
        bool CanPublishThis(T data);

        void Publish(T data);
    }
}