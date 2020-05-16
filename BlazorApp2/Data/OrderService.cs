using BlazorApp2.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        public async Task<IEnumerable<Order>> ListAsync(
            string locationId)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, $"api/locations/{locationId}/orders");

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var orders =
                await httpResponseMessage.Content.ReadAsJsonAsync<IEnumerable<Order>>();

            return orders;
        }
    }
}
