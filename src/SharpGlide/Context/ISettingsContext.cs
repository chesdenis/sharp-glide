namespace SharpGlide.Context
{
    public interface ISettingsContext
    {
        TSettings GetByKey<TSettings>(string keyPath);
    }
}