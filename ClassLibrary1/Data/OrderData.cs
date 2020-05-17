using Newtonsoft.Json;
using System;

namespace ClassLibrary1.Data
{
    public class OrderData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerPhoneNumber")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("locationIdAndDate")]
        public string LocationIdAndDate { get; set; }

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }

        [JsonProperty("arrivedAt")]
        public DateTime? ArrivedAt { get; set; }

        [JsonProperty("deliveredAt")]
        public DateTime? DeliveredAt { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }
    }
}
