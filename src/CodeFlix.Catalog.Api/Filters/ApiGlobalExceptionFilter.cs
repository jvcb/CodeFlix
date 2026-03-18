using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeFlix.Catalog.Api.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ApiGlobalExceptionFilter(IHostEnvironment env)
        => _env = env;

    public void OnException(ExceptionContext context)
    {
        ProblemDetails details;

        if (context.Exception is EntityValidationException validationException)
        {
            details = new ProblemDetails
            {
                Title = "One or more validation errors occurred",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = validationException.Message,
                Type = "UnprocessableEntity"
            };
            context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        else if (context.Exception is NotFoundException notFoundException)
        {
            details = new ProblemDetails
            {
                Title = "Not found",
                Status = StatusCodes.Status404NotFound,
                Detail = notFoundException.Message,
                Type = "NotFound"
            };
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else
        {
            details = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = _env.IsDevelopment() ? context.Exception.ToString() : "An unexpected error occurred",
                Type = "UnexpectedError"
            };
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        context.Result = new ObjectResult(details)
        {
            StatusCode = context.HttpContext.Response.StatusCode
        };
        context.ExceptionHandled = true;
    }
}
