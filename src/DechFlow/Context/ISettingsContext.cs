namespace DechFlow.Context
{
    public interface ISettingsContext
    {
        TSettings GetByKey<TSettings>(string keyPath);
    }
}