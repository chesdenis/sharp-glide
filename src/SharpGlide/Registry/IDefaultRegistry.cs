using System;

namespace SharpGlide.Registry
{
    public interface IDefaultRegistry
    {
        void Set<T>(Func<object> builder);
        void SetNamed<T>(Func<object> builder, string name);
        T Get<T>();
        T GetNamed<T>(string name);
    }
}