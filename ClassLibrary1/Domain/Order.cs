using ClassLibrary1.Data;
using System;

namespace ClassLibrary1.Domain
{
    public class Order
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime CreatedOn { get; set; }

        public Order()
        {
        }

        public Order(
            OrderData orderData)
        {
            this.Id = orderData.Id;
            this.OrderId = orderData.OrderId;
            this.Date = orderData.Date;
            this.CustomerName = orderData.CustomerName;
            this.CustomerPhoneNumber = orderData.CustomerPhoneNumber;
            this.LocationId = orderData.LocationId;
            this.ReadyAt = orderData.ReadyAt;
            this.ArrivedAt = orderData.ArrivedAt;
            this.DeliveredAt = orderData.DeliveredAt;
            this.CreatedOn = orderData.CreatedOn;
        }
    }
}