using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Web.Services;

namespace Core.Lib.Web.Controllers
{
    public abstract class ControllerWebModel<TService, TModel> : ControllerWebApi<TService>
                                                                    where TService : IService<TModel>
                                                                    where TModel   : ModelBase
    {
        //TODO: add some general cases when working with models.
        //TODO: add some work with model collections
        //TODO: add test\stub logic here
    }
}
