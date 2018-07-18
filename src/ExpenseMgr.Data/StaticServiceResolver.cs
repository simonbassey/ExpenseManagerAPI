using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ExpenseMgr.Data
{
    public interface IConfigSettingsReader
    {
        IConfiguration GetConfiguration();
    }
    public class StaticServiceResolver
    {
        public static StaticServiceResolver Instance { get; private set; }
        public IServiceProvider _serviceProvider { get; }

        private StaticServiceResolver(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public static StaticServiceResolver Register(IServiceProvider provider)
        {
            Instance = Instance ?? new StaticServiceResolver(provider);
            return Instance;
        }

        public static T Resolve<T>()
        {
            return (T)Instance._serviceProvider.GetService(typeof(T));
        }
    }
}
