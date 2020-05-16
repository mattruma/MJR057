using Newtonsoft.Json;
using System;

namespace FunctionApp2
{
    public class Function5Data
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

        [JsonProperty("arrivedAt")]
        public DateTime? ArrivedAt { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        public Function5Data()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Function5Data(
            Function5Request function5Request)
        {
            this.Id = function5Request.Id;
            this.OrderId = function5Request.OrderId;
            this.Date = function5Request.Date;
            this.CustomerName = function5Request.CustomerName;
            this.CustomerPhoneNumber = function5Request.CustomerPhoneNumber;
            this.ReadyAt = function5Request.ReadyAt;
            this.ArrivedAt = function5Request.ArrivedAt;
            this.CreatedOn = function5Request.CreatedOn;
        }
    }
}
