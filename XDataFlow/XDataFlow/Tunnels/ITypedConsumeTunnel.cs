namespace XDataFlow.Tunnels
{
    public interface ITypedConsumeTunnel<out T>
    {
        T Consume();
    }
}