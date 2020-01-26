using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Web.Services
{
    public class ServiceRepository : IServiceRepository
    {
        private Dictionary<Type, IService> _services;

        public ServiceRepository()
        {
            _services = new Dictionary<Type, IService>();
        }

        public void AddService<T>(IService service) where T : IService
        {
            _services.Add(typeof(T), service);
        }

        public void AddServices(IEnumerable<IService> services)
        {
            services.ToList().ForEach(service => _services?.Add(service.GetType(), service));
        }

        public T GetService<T>() where T : IService
        {
            if(_services.TryGetValue(typeof(T), out IService foundService))
            {
                return (T) foundService;
            }
            else
            {
                //null
                return default(T);
            }
        }

        public IEnumerable<IService> GetServices()
        {
            return _services.Values;
        }

        public void RemoveService<T>() where T : IService
        {
            _services.Remove(typeof(T));
        }
    }
}
