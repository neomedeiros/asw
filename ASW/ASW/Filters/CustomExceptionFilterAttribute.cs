using System.Net;
using ASW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASW.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.Result = new JsonResult(new ErrorModel
            {
                Message = exception.Message,
                StatusCode = HttpStatusCode.BadRequest
            });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        }
    }
}