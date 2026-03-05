

namespace Application.DTOs.ManageSales
{
    public class FiltersDto
    {
        public List<RouteDto> Routes { get; set; } = [];
        public List<StateTicketDto> StateTickets { get; set; } = [];
    }

    public class RouteDto
    {
        public int IdRoute { get; set; }
        public required string Name { get; set; }
    }

    public class StateTicketDto
    {
        public int IdState { get; set; }
        public string? Name { get; set; }
    }
}
