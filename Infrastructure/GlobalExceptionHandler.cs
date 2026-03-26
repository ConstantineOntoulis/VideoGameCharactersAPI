using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

//Centralized handler for unexpected server-side exceptions
namespace VideoGameCharacterApi.Infrastructure
{
    // ": IExceptionHandler" means this class promises to follow the contract
    //"(ILogger<GlobalExceptionHandler> logger)" is a primary constructor.
    //ASP.NET Core's dependency injection system will automatically provide a logger instance when this class is created.
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        //Method required by IExceptionHandler
        //Converts unhandled exceptions into a predictable 500 ProblemDetails response.
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,  //Represents the current HTTP request/response context
            Exception exception,  //The exception that was thrown somewhere in the app
            CancellationToken cancellationToken)  //Lets ASP.NET Core cancel the work if needed
        {
            logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
                //The HTTP method of the failing request
                httpContext.Request.Method,
                //The route/path of the failing request
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            //Create a ProblemDetails object. Standard structured format for HTTP API errors
            var problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                Title = "An unexpected server error occurred.",
                Status = StatusCodes.Status500InternalServerError, //Unexpected failure 500
                Detail = "The server encountered an unexpected error while processing the request.",
                Instance = httpContext.Request.Path  //Identifies the specific request path where the problem happened.
            };

            //"traceId" helps correlate this client-facing error with server logs.
            //can search your logs for that same trace identifier and find the exact logged exception.
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            //Explicitly set the HTTP status code on the response to 500
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            //Write the ProblemDetails object as JSON to the HTTP response body instead of random text
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}