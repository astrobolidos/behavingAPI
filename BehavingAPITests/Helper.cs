using System;
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
            return await _testServer.CreateClient().PostAsync(requestUri, GetContent(payload));
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, string payload)
        {
            return await _testServer.CreateClient().PutAsync(requestUri, GetContent(payload));
        }

        private static StringContent GetContent(string payload)
        {
            return new StringContent(
                payload,
                Encoding.UTF8,
                "application/json");
        }


    }
}
