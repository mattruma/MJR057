using BlazorApp1.Helpers;
using Newtonsoft.Json;
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
                new HttpRequestMessage(HttpMethod.Get, $"api/orders/{id}");

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var order =
                await httpResponseMessage.Content.ReadAsJsonAsync<Order>();

            return order;
        }

        public async Task<Order> UpdateHasArrivedByIdAsync(
            string id,
            bool hasArrived)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"api/orders/{id}");

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
