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
            OrderReceiveData orderReceiveData)
        {
            this.Id = orderReceiveData.Id;
            this.Date = orderReceiveData.Date;
            this.CustomerName = orderReceiveData.CustomerName;
            this.CustomerPhoneNumber = orderReceiveData.CustomerPhoneNumber;
            this.LocationId = orderReceiveData.LocationId;
            this.ReadyAt = orderReceiveData.ReadyAt;
            this.CreatedOn = orderReceiveData.CreatedOn;
        }
    }
}
