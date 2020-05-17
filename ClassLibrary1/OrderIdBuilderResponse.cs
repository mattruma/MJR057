using System;

namespace ClassLibrary1
{
    public class OrderIdBuilderResponse
    {
        public string Id { get; internal set; }
        public string LocationIdAndDate { get; internal set; }

        public OrderIdBuilderResponse(
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
    }
}
