using System;

namespace BlazorApp2.Data
{
    public class Order
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public string Status
        {
            get
            {
                if (this.HasBeenDelivered) return "Delivered";

                if (this.HasArrived) return "Arrived";

                return "Open";
            }
        }

        public int Priority
        {
            get
            {
                if (this.HasBeenDelivered) return 1;

                if (this.HasArrived) return 3;

                return 2;
            }
        }

        public DateTime ReadyAt { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public bool HasArrived => this.ArrivedAt.HasValue;

        public DateTime? DeliveredAt { get; set; }

        public bool HasBeenDelivered => this.DeliveredAt.HasValue;

        public DateTime CreatedOn { get; set; }
    }
}
