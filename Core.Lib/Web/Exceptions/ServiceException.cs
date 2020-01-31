using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core.Lib.Web.Exceptions
{
    public class ServiceException : Exception
    {
        public string UserMessage { get; set; }

        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public Exception OriginalException { get; set; }

        public ServiceException(HttpStatusCode statusCode, string userMessage, string errorMessage, Exception exception)
            : base(errorMessage, exception)
        {
            StatusCode = statusCode;
            UserMessage = userMessage;
            ErrorMessage = errorMessage ?? exception?.Message ?? string.Empty;
            OriginalException = exception;
        }

        public ServiceException(HttpStatusCode statusCode, string userMessage, Exception exception)
            :this(statusCode, userMessage, null, exception)
        {

        }
    }
}
