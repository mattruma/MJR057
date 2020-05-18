using BlazorApp2.Helpers;
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

        public async Task<PagedResponse<Location>> ListAsync()
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, $"deliver/locations");

            var httpResponseMessage =
                await _httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var locations =
                await httpResponseMessage.Content.ReadAsJsonAsync<PagedResponse<Location>>();

            return locations;
        }
    }
}
