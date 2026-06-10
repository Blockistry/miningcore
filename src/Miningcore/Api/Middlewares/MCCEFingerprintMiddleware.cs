using Microsoft.AspNetCore.Http;

namespace Miningcore.Api.Middlewares;

/// <summary>
/// Injects a hidden fingerprint header into all API responses so MCCE instances
/// can be identified vs original miningcore. This is intentionally subtle —
/// not visible in browser devtools "Network" tab unless you expand headers.
/// </summary>
public class MCCEFingerprintMiddleware
{
    public MCCEFingerprintMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private readonly RequestDelegate _next;

    public async Task Invoke(HttpContext context)
    {
        // Inject fingerprint header on every response
        context.Response.OnStarting(() =>
        {
            // Original miningcore never sets this header — presence = MCCE fork
            context.Response.Headers["X-Pool-Generator"] = "mcce";

            // Also override Server header to be less identifiable to casual scans
            // but distinctive to anyone who knows what to look for
            context.Response.Headers["Server"] = "miningcore";

            return Task.CompletedTask;
        });

        await _next(context);
    }
}
