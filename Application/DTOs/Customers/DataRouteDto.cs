

namespace Application.DTOs.Customers
{
    public class DataRouteDto
    {
        public int IdRoute { get; set; }
        public required string NameRoute { get; set; }
        public double Price { get; set; }
        public required List<DepartureTimeByQueueDto> DepartureHour { get; set; }

    }

    public class DepartureTimeByQueueDto
    {
        public int Id { get; set; }
        public DateTime DepartureHour { get; set; }
        public VehicleDto? Vehicle { get; set; }
    }

    public class VehicleDto
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public int AvailableSeats { get; set; }
    }

}
