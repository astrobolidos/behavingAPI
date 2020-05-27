using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BehavingAPI.Middleware
{
    public class PerformanceLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _stopWatch;
        private readonly ILogger _logger;

        public PerformanceLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _stopWatch = new Stopwatch();
            _next = next;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _stopWatch.Start();
            await _next(httpContext);
            _stopWatch.Stop();

            if (_stopWatch.ElapsedMilliseconds > 1200)
                httpContext.Response.Headers.Add(
                    "PerformanceWarning",
                    $"Request to {httpContext.GetEndpoint()} taking too long. {_stopWatch.ElapsedMilliseconds}ms");
        }
    }
}
