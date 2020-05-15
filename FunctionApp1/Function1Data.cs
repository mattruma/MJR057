using System;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class Function1Data
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("arrivedAt")]
        public DateTime? ArrivedAt { get; set; }
    }
}
