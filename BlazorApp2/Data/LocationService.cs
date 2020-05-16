using BlazorApp2.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;

        public LocationService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        public async Task<IEnumerable<Location>> ListAsync()
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, $"api/locations");

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var orders =
                await httpResponseMessage.Content.ReadAsJsonAsync<IEnumerable<Location>>();

            return orders;
        }
    }
}
