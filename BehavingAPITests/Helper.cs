using System.Net.Http;
using System.Threading.Tasks;
using BehavingAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace BehavingAPITests
{
    public class Helper
    {
        public static async Task<HttpResponseMessage> GetResult(string requestUri)
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            return await server.CreateClient().GetAsync(requestUri);
        }
    }
}
