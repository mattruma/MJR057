using ClassLibrary1.Data;
using System;

namespace FunctionApp1
{
    public class CustomerArriveResponse
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public DateTime ReadyAt { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public CustomerArriveResponse()
        {
        }

        public CustomerArriveResponse(
            OrderData orderData)
        {
            this.Id = orderData.Id;
            this.Date = orderData.Date;
            this.CustomerName = orderData.CustomerName;
            this.CustomerPhoneNumber = orderData.CustomerPhoneNumber;
            this.ReadyAt = orderData.ReadyAt;
            this.ArrivedAt = orderData.ArrivedAt;
        }
    }
}
