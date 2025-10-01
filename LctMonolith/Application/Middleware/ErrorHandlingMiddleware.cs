using System.Net;
using System.Text.Json;
using Serilog;

namespace LctMonolith.Application.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (OperationCanceledException)
        {
            if (!ctx.Response.HasStarted)
            {
                ctx.Response.StatusCode = 499;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception");
            if (ctx.Response.HasStarted)
            {
                throw;
            }
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var payload = new { error = new { message = ex.Message, traceId = ctx.TraceIdentifier } };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
