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
            CustomerArriveData customerArriveData)
        {
            this.Id = customerArriveData.Id;
            this.Date = customerArriveData.Date;
            this.CustomerName = customerArriveData.CustomerName;
            this.CustomerPhoneNumber = customerArriveData.CustomerPhoneNumber;
            this.ReadyAt = customerArriveData.ReadyAt;
            this.ArrivedAt = customerArriveData.ArrivedAt;
        }
    }
}
