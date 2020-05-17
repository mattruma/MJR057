using System;

namespace FunctionApp1.Helpers
{
    public class OrderIdParserResponse
    {
        public string Id { get; internal set; }
        public string LocationIdAndDate { get; internal set; }

        public OrderIdParserResponse(
            string id,
            string locationIdAndDate)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException($"{nameof(id)} cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(locationIdAndDate))
            {
                throw new ArgumentNullException($"{nameof(locationIdAndDate)} cannot be empty.");
            }

            this.Id = id;
            this.LocationIdAndDate = locationIdAndDate;
        }

        public OrderIdParserResponse(
            string id,
            string locationId,
            string date)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException($"{nameof(id)} cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(locationId))
            {
                throw new ArgumentNullException($"{nameof(locationId)} cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(date))
            {
                throw new ArgumentNullException($"{nameof(date)} cannot be empty.");
            }

            this.Id = id;
            this.LocationIdAndDate = $"{locationId}-{date}";
        }
    }
}
