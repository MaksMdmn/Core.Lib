using System.Collections.Generic;

namespace Core.Lib.Web.Services
{
    public static class ServiceProvider
    {
        private static IServiceRepository _repository = new ServiceRepository();

        public static void RegisterService<T>(IService service) where T : IService
        {
            _repository.AddService<T>(service);
        }

        public static void UnregisterService<T>(IService service) where T : IService
        {
            _repository.RemoveService<T>();
        }

        public static T RetrieveService<T>() where T : IService
        {
            return _repository.GetService<T>();
        }
    }
}
