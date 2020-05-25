using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UrlShortener.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        public void OnActionExecuting(ActionExecutingContext context) { }

        private void BuildError(HttpStatusCode statusCode, ActionExecutedContext context)
        {
            Error error = new Error
            {
                Code = (int) statusCode,
                Message = $"{statusCode.ToString()}. " + context.Exception.Message
            };

            context.Result = new JsonResult(error)
            {
                StatusCode = error.Code
            };
        }
        
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case ApiNotFoundException _:
                {
                    BuildError(HttpStatusCode.NotFound, context);
                    context.ExceptionHandled = true;
                    break;
                }
                case ApiBadRequestException _:
                {
                    BuildError(HttpStatusCode.BadRequest, context);
                    context.ExceptionHandled = true;
                    break;
                }
            }
        }
    }
}