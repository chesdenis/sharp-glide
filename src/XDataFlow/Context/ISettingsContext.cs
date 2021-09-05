namespace XDataFlow.Context
{
    public interface ISettingsContext
    {
        TSettings GetByKey<TSettings>(string keyPath);
    }
}