using System;
using System.Collections.Generic;

namespace Services
{
    public class AllServices
    {
        private static AllServices instance;
        public static AllServices Instance => instance ??= new AllServices();
        
        private readonly Dictionary<Type, IService> _container = new();
        
        public TService Get<TService>() where TService : IService =>
            (TService)_container[typeof(TService)];
        
        public void Register<TService>(TService service) where TService : IService =>
            _container.Add(typeof(TService), service);
        
        public void Dispose()
        {
            instance = null;
        }
    }
}