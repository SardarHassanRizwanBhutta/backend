using System.Net;
using System.Text.Json;

// RequestDelegate process https request and represent next middleware in the pipeline
// When middleware component gets executed it executes operations before and after calling the next middlware in the pipeline
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // Passing the context which is an instance of HtppContext representing current http context
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Something went wrong");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ResponseObject<object>("Something went wrong", null));
        }
    }
}

    // public class UnauthrorizedException:Exception

    // {
    //     public UnauthrorizedException(string message): base(message)
    //     {

    //     }
    // }