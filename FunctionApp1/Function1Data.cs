using Newtonsoft.Json;
using System;

namespace FunctionApp1
{
    public class Function1Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("arrivedAt")]
        public DateTime? ArrivedAt { get; set; }

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }
    }
}
