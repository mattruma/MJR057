using System;

namespace FunctionApp1
{
    public class Function2Response
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public DateTime ReadyAt { get; set; }

        public Function2Response()
        {
        }

        public Function2Response(
            Function2Data function2Data)
        {
            this.Id = function2Data.Id;
            this.OrderId = function2Data.OrderId;
            this.CustomerName = function2Data.CustomerName;
            this.ArrivedAt = function2Data.ArrivedAt;
            this.ReadyAt = function2Data.ReadyAt;
        }
    }
}
