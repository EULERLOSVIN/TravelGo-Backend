

namespace Application.DTOs.ManageSales
{
    public class SaleDto
    {
        public int IdTicket { get; set; }
        public DateOnly? Date { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Dni { get; set; }
        public required string Route { get; set; }
        public int SeatNumber { get; set; }
        public decimal? UnitPrice { get; set; }
        public required string PaymentMethod { get; set; }
    }
}
