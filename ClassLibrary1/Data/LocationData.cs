using Newtonsoft.Json;

namespace FunctionApp2
{
    public class LocationData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
