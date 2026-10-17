using Auth0MvcDemo.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth0MvcDemo.Filters;

public class ApiForbiddenExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ApiForbiddenException exception)
            return;

        context.Result = new ViewResult
        {
            ViewName = "~/Views/Shared/Forbidden.cshtml",
            StatusCode = StatusCodes.Status403Forbidden,
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                context.ModelState)
            {
                ["Message"] = exception.Message
            }
        };

        context.ExceptionHandled = true;
    }
}
