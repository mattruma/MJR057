using System;

namespace FunctionApp2
{
    public class OrderReceiveRequest
    {
        public string OrderId { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string LocationId { get; set; }

        public DateTime ReadyAt { get; set; }
    }
}
