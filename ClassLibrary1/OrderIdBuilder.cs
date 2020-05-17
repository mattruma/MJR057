using System;
using System.Text.RegularExpressions;

namespace ClassLibrary1
{
    public class OrderIdBuilder
    {
        public OrderIdBuilderResponse Build(
            string orderId,
            string locationId,
            DateTime date)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentNullException($"{nameof(orderId)} cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(locationId))
            {
                throw new ArgumentNullException($"{nameof(locationId)} cannot be empty.");
            }

            var locationIdAndDate = $"{locationId}-{date:yyyyMMdd}";

            Regex regex;

            regex = new Regex("[^a-zA-Z0-9]");

            orderId = regex.Replace(orderId, "").ToUpper();

            var id = $"{locationIdAndDate}-{orderId}";

            var orderIdBuilderResponse =
                new OrderIdBuilderResponse(id, locationIdAndDate);

            return orderIdBuilderResponse;
        }
    }
}
