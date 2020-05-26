using System.Net.Http;
using System.Threading.Tasks;
using BehavingAPI.Behaviour;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BehavingAPITests
{
    public class Helper
    {
        public static async Task<HttpResponseMessage> GetResult(string requestUri, ILogger logger)
        {
            var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton(logger);
                        })
                        .Configure(app =>
                        {
                            app.UseMiddleware<PerformanceBehaviour>();
                        });
                })
                .StartAsync();

            return await host.GetTestServer().CreateClient().GetAsync(requestUri);
        }
    }
}
