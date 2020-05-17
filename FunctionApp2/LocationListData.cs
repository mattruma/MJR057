using Newtonsoft.Json;

namespace FunctionApp2
{
    public class LocationListData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
