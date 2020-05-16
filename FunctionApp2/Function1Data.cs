﻿using Newtonsoft.Json;
using System;

namespace FunctionApp2
{
    public class Function1Data
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

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        public Function1Data()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}
