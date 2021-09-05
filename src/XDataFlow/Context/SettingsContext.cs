using System;

namespace XDataFlow.Context
{
    public class SettingsContext : ISettingsContext
    {
        public TSettings GetByKey<TSettings>(string keyPath)
        {
            throw new NotImplementedException();
        }
    }
}