using System;

namespace FunctionApp2
{
    public class OrderDeliverResponse
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public OrderDeliverResponse()
        {
        }

        public OrderDeliverResponse(
            OrderDeliverData orderDeliverData)
        {
            this.Id = orderDeliverData.Id;
            this.Date = orderDeliverData.Date;
            this.CustomerName = orderDeliverData.CustomerName;
            this.CustomerPhoneNumber = orderDeliverData.CustomerPhoneNumber;
            this.ReadyAt = orderDeliverData.ReadyAt;
            this.ArrivedAt = orderDeliverData.ArrivedAt;
            this.DeliveredAt = orderDeliverData.DeliveredAt;
        }
    }
}
