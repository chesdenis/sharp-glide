using System;

namespace DechFlow.Context
{
    public class SettingsContext : ISettingsContext
    {
        public TSettings GetByKey<TSettings>(string keyPath)
        {
            throw new NotImplementedException();
        }
    }
}