using Newtonsoft.Json;
using System;

namespace FunctionApp1
{
    public class OrderRetrieveData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerPhoneNumber")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }
    }
}
