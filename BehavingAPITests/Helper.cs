using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BehavingAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace BehavingAPITests
{
    public class Helper
    {
        private static TestServer _testServer;

        public static void Init()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }


        public static async Task<HttpResponseMessage> Get(string requestUri)
        {
            return await _testServer.CreateClient().GetAsync(requestUri);
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, string payload)
        {
            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json");

            return await _testServer.CreateClient().PostAsync(requestUri, content);
        }
    }
}
