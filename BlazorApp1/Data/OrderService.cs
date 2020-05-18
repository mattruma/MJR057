using BlazorApp1.Helpers;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        public async Task<Order> GetByIdAsync(
            string id)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, $"pickup/orders/{id}");

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var order =
                await httpResponseMessage.Content.ReadAsJsonAsync<Order>();

            return order;
        }

        [SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public async Task<Order> ArriveAsync(
            string id,
            bool hasArrived)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"pickup/orders/{id}/arrive");

            httpRequestMessage.Content =
                new StringContent(
                    JsonConvert.SerializeObject(new { HasArrived = hasArrived }));

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var order =
                await httpResponseMessage.Content.ReadAsJsonAsync<Order>();

            return order;
        }
    }
}
