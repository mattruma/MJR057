using System;

namespace ClassLibrary1
{
    public class OrderIdParser
    {
        public OrderIdParserResponse Parse(
            string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentNullException($"{nameof(orderId)} cannot be empty.");
            }

            var values = orderId.Split("-");

            if (values.Length != 3)
            {
                throw new ArgumentOutOfRangeException($"{nameof(orderId)} must be in the format of LocationId-Date-OrderId.");
            }

            var locationId = values[0];
            var date = values[1];

            return new OrderIdParserResponse(orderId, locationId, date);
        }
    }
}
