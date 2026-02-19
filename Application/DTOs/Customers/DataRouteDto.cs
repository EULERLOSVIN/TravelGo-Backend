

namespace Application.DTOs.Customers
{
    public class DataRouteDto
    {
        public int IdRoute { get; set; }
        public required string NameRoute { get; set; }
        public int AvailableSeat { get; set; }
        public double Price { get; set; }
        public VehicleDto? Vehicles { get; set; }

    }

    public class VehicleDto
    {
        public int Id { get; set; }
        public string?  Photo { get; set; }
    }
}
