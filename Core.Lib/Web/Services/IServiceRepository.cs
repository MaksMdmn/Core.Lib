using System.Collections.Generic;

namespace Core.Lib.Web.Services
{
    public interface IServiceRepository
    {
        T GetService<T>() where T : IService;

        IEnumerable<IService> GetServices();

        void AddService<T>(IService service) where T : IService;

        void AddServices(IEnumerable<IService> service);

        void RemoveService<T>() where T : IService;

    }
}
