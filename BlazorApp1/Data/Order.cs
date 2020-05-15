using System;

namespace BlazorApp1.Data
{
    public class Order
    {
        public string OrderId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public bool HasArrived => this.ArrivedAt.HasValue;
    }
}
