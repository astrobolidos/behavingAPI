using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BehavingAPI.Behaviour
{
    public class PerformanceBehaviour
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _stopWatch;

        public PerformanceBehaviour(RequestDelegate next)
        {
            _stopWatch = new Stopwatch();
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger logger)
        {
            _stopWatch.Start();
            await _next(httpContext);
            _stopWatch.Stop();

            if (_stopWatch.ElapsedMilliseconds > 1200)
                logger.LogError($"Request to {httpContext.GetEndpoint()} taking too long. {_stopWatch.ElapsedMilliseconds}ms");
        }
    }
}
