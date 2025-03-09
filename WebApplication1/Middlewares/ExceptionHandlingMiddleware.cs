using System.Net;
using System.Text.Json;

// RequestDelegate process|handles https request and represent next middleware in the pipeline
// When middleware component gets executed it executes operations before and after calling the next middlware in the pipeline
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // Passing the httpcontext which is an instance of HttpContext representing current http context to the next middleware in the app pipeline
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An unexpected error");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ResponseObject<object>("An unexpected error", null));
        }
    }
}