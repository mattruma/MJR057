namespace FunctionApp2
{
    public class Function1Response : Function1Data
    {
        public Function1Response()
        {
        }

        public Function1Response(
            Function1Data function1Data)
        {
            this.Id = function1Data.Id;
            this.OrderId = function1Data.OrderId;
            this.Date = function1Data.Date;
            this.CustomerName = function1Data.CustomerName;
            this.CustomerPhoneNumber = function1Data.CustomerPhoneNumber;
            this.LocationId = function1Data.LocationId;
            this.ReadyAt = function1Data.ReadyAt;
            this.CreatedOn = function1Data.CreatedOn;
        }
    }
}
