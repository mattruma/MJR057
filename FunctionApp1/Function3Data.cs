using Newtonsoft.Json;
using System;

namespace FunctionApp1
{
    public class Function3Data
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

        [JsonProperty("readyAt")]
        public DateTime ReadyAt { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        public Function3Data()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Function3Data(
            Function3Request function3Request)
        {
            this.Id = function3Request.Id;
            this.OrderId = function3Request.OrderId;
            this.Date = function3Request.Date;
            this.CustomerName = function3Request.CustomerName;
            this.CustomerPhoneNumber = function3Request.CustomerPhoneNumber;
            this.ReadyAt = function3Request.ReadyAt;
            this.CreatedOn = function3Request.CreatedOn;
        }
    }
}
