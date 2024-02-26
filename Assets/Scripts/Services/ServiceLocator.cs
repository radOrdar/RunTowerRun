using System;
using System.Collections.Generic;

namespace Services
{
    public class ServiceLocator : IDisposable
    {
        private static ServiceLocator instance;
        public static ServiceLocator Instance => instance ??= new ServiceLocator();
        
        private readonly Dictionary<Type, IService> _container = new();
        
        public TService Get<TService>() where TService : IService
        {
            _container.TryGetValue(typeof(TService), out IService value);
            return (TService) value;
        }

        public void Register<TService>(TService service) where TService : IService =>
            _container.Add(typeof(TService), service);
        
        public void Dispose()
        {
            instance = null;
        }
    }
}