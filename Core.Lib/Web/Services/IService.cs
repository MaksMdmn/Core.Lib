using Core.Lib.Backend.Common.Abstract;

namespace Core.Lib.Web.Services
{
    public interface IService<T> : IService where T : ModelBase
    {

    }

    public interface IService
    {

    }
}
