using DocumentApp.API.Exceptions;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DocumentApp.API.Filters
{
    public class DocumentAppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as DocumentAppException;
            if (exception != null)
            {
                context.Response = context.Request.CreateErrorResponse(exception.StatusCode, exception.Message);
            }
        }
    }
}