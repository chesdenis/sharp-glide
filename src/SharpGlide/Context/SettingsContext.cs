using System;

namespace SharpGlide.Context
{
    public class SettingsContext : ISettingsContext
    {
        public TSettings GetByKey<TSettings>(string keyPath)
        {
            throw new NotImplementedException();
        }
    }
}