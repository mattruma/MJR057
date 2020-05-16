using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp2.Helpers
{
    public static class HttpContentExtensions
    {
        public static async Task<TEntity> ReadAsJsonAsync<TEntity>(
            this HttpContent httpContent)
        {
            var content =
                await httpContent.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TEntity>(
                content);
        }
    }
}
