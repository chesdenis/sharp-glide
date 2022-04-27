namespace SharpGlide.Flow
{
    public interface IConfigurationEntryProvider
    {
        ConfigurationEntry Parse(string contents);

        string Read(string pointer);
    }
}