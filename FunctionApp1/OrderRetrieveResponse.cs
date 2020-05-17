using System;

namespace FunctionApp1
{
    public class OrderRetrieveResponse
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime CreatedOn { get; set; }

        public OrderRetrieveResponse()
        {
        }

        public OrderRetrieveResponse(
            OrderRetrieveData orderRetrieveData)
        {
            this.Id = orderRetrieveData.Id;
            this.Date = orderRetrieveData.Date;
            this.CustomerName = orderRetrieveData.CustomerName;
            this.CustomerPhoneNumber = orderRetrieveData.CustomerPhoneNumber;
            this.LocationId = orderRetrieveData.LocationId;
            this.ReadyAt = orderRetrieveData.ReadyAt;
            this.CreatedOn = orderRetrieveData.CreatedOn;
        }
    }
}
