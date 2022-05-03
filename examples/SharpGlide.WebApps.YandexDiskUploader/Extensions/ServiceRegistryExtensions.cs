using System;
using Microsoft.Extensions.DependencyInjection;

namespace SharpGlide.WebApps.YandexDiskUploader.Extensions
{
    public static class ServiceRegistryExtensions
    {
        public static void AddTunnel<TTunnel, TImpl>(this IServiceCollection serviceCollection, Func<TTunnel, TImpl> implFunc) 
            where TTunnel : class 
            where TImpl : class
        {
            serviceCollection.AddTransient<TTunnel>();
            serviceCollection.AddTransient<TImpl>(provider =>
            {
                var tunnel = provider.GetService<TTunnel>();
                return implFunc(tunnel);
            });
        }
    }
}