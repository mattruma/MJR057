using Newtonsoft.Json;
using System;

namespace FunctionApp2
{
    public class Function2Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerPhoneNumber")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }

        [JsonProperty("arrivedAt")]
        public DateTime? ArrivedAt { get; set; }

        [JsonProperty("deliveredAt")]
        public DateTime? DeliveredAt { get; set; }
    }
}
