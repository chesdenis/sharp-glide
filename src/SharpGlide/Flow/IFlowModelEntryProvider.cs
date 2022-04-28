namespace SharpGlide.Flow
{
    public interface IFlowModelEntryProvider
    {
        ConfigurationEntry Parse(string contents);

        string Read(string pointer);
    }
}