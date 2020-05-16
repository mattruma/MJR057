using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

namespace FunctionApp1
{
    public class Function3Request
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

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        public Function3Request()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Function3Request(
            Document document)
        {
            this.Id = document.GetPropertyValue<string>("id");
            this.OrderId = document.GetPropertyValue<string>("orderId");
            this.Date = document.GetPropertyValue<DateTime>("date");
            this.CustomerName = document.GetPropertyValue<string>("customerName");
            this.CustomerPhoneNumber = document.GetPropertyValue<string>("customerPhoneNumber");
            this.LocationId = document.GetPropertyValue<string>("locationId");
            this.ReadyAt = document.GetPropertyValue<DateTime>("readyAt");
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}
