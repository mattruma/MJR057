using System;

namespace FunctionApp1
{
    public class Function1Response
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? ArrivedAt { get; set; }

        public DateTime ReadyAt { get; set; }

        public Function1Response()
        {
        }

        public Function1Response(
            Function1Data function1Data)
        {
            this.Id = function1Data.Id;
            this.OrderId = function1Data.OrderId;
            this.CustomerName = function1Data.CustomerName;
            this.ArrivedAt = function1Data.ArrivedAt;
            this.ReadyAt = function1Data.ReadyAt;
        }
    }
}
