using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace CandidateManagement_API.Pipeline.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception?.InnerException?.Message ?? exception?.Message ?? "Unexpected error occurred.";
        var exceptionStack = exception?.InnerException?.StackTrace ?? exception?.StackTrace ?? "";
 
        var status = exception switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new 
        {
            ExceptionMessage = exceptionMessage,
            ExceptionStack = exceptionStack
        };

        var serializedResponse = JsonSerializer.Serialize(response);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)status;

        await httpContext.Response.WriteAsync(serializedResponse, cancellationToken);

        return true;
    }
}