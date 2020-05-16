using System;

namespace FunctionApp2
{
    public class OrderReceivedResponse
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime CreatedOn { get; set; }

        public OrderReceivedResponse()
        {
        }

        public OrderReceivedResponse(
            OrderReceivedData orderReceivedData)
        {
            this.Id = orderReceivedData.Id;
            this.Date = orderReceivedData.Date;
            this.CustomerName = orderReceivedData.CustomerName;
            this.CustomerPhoneNumber = orderReceivedData.CustomerPhoneNumber;
            this.LocationId = orderReceivedData.LocationId;
            this.ReadyAt = orderReceivedData.ReadyAt;
            this.CreatedOn = orderReceivedData.CreatedOn;
        }
    }
}
