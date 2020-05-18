using BlazorApp2.Helpers;
using System;
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

        public async Task<PagedResponse<Order>> ListAsync(
            string locationId,
            DateTime? date = null)
        {
            var requestUri =
                $"deliver/locations/{locationId}/orders";

            if (!date.HasValue)
            {
                date = DateTime.UtcNow;
            }

            requestUri += $"?date={date.Value:d}";
            requestUri += $"&pageSize=99999999";

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, requestUri);

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var orders =
                await httpResponseMessage.Content.ReadAsJsonAsync<PagedResponse<Order>>();

            return orders;
        }
    }
}
