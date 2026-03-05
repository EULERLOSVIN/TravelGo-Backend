

namespace Application.DTOs.Driver
{
    public class TripsMadeDto
    {
        public int IdTrip { get; set; }
        public required string NameRoute { get; set; }
        public DateOnly? DepartureDate { get; set; }
        public int NumberPassengers { get; set; }
    }
}
