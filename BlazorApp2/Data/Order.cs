using System;

namespace BlazorApp2.Data
{
    public class Order
    {
        public string Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public bool HasArrived => this.ArrivedAt.HasValue;

        public DateTime? DeliveredAt { get; set; }

        public bool HasBeenDelivered => this.DeliveredAt.HasValue;

        public DateTime CreatedOn { get; set; }
    }
}
