namespace XDataFlow.Context
{
    public interface ISettingsContext
    {
        TSettings GetByKey<TSettings>(string keyPath);
    }

    public class SettingsContext : ISettingsContext
    {
        public TSettings GetByKey<TSettings>(string keyPath)
        {
            throw new System.NotImplementedException();
        }
    }
}