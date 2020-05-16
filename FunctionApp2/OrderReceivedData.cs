using Newtonsoft.Json;
using System;

namespace FunctionApp2
{
    public class OrderReceivedData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

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

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        public OrderReceivedData()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}
