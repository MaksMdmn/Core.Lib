using Core.Lib.Web.Exceptions;
using Core.Lib.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Net;

namespace Core.Lib.Web.Controllers
{
    public abstract class ControllerWebApi<TService> : Controller where TService : IService
    {
        protected TService  Service => ServiceProvider.RetrieveService<TService>();

        protected Logger    Logger => LogManager.GetCurrentClassLogger();

        protected HttpStatusCode DefaultErrorCode { get; set; } = HttpStatusCode.ServiceUnavailable;

        protected string         DefaultErrorMessage { get; set; } = "Unknown error, already working on it... Apologies, pal :(";

        protected virtual IActionResult ExecuteAction(Action serviceAction)
        {
            try
            {
                serviceAction?.Invoke();

                return Ok();
            }
            catch (ServiceException ex)
            {
                return StatusCode((int) ex.StatusCode, ex.UserMessage);
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Unhaneled error while executing: {nameof(serviceAction)}, Error: {ex.Message} {Environment.NewLine} {ex.StackTrace}");

                return StatusCode((int) DefaultErrorCode, DefaultErrorMessage);
            }
        }

        protected virtual IActionResult ExecuteAction<TResult>(Func<TResult> serviceAction) where TResult : class
        {
            try
            {
                return Ok(serviceAction?.Invoke());
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.UserMessage);
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Unhaneled error while executing: {nameof(serviceAction)}, Error: {ex.Message} {Environment.NewLine} {ex.StackTrace}");

                return StatusCode((int)DefaultErrorCode, DefaultErrorMessage);
            }
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
