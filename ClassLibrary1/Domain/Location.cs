using FunctionApp2;

namespace ClassLibrary1.Domain
{
    public class Location
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Location()
        {
        }

        public Location(
            LocationData locationData)
        {
            this.Id = locationData.Id;
            this.Name = locationData.Name;
        }
    }
}
