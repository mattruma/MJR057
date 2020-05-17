using ClassLibrary1.Data;
using System;

namespace FunctionApp2
{
    public class OrderReceiveResponse
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime CreatedOn { get; set; }

        public OrderReceiveResponse()
        {
        }

        public OrderReceiveResponse(
            OrderData orderData)
        {
            this.Id = orderData.Id;
            this.Date = orderData.Date;
            this.CustomerName = orderData.CustomerName;
            this.CustomerPhoneNumber = orderData.CustomerPhoneNumber;
            this.LocationId = orderData.LocationId;
            this.ReadyAt = orderData.ReadyAt;
            this.CreatedOn = orderData.CreatedOn;
        }
    }
}
