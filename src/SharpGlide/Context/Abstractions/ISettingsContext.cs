namespace SharpGlide.Context.Abstractions
{
    public interface ISettingsContext
    {
        TSettings GetByKey<TSettings>(string keyPath);
    }
}