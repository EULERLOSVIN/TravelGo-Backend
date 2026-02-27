

namespace Application.DTOs.Customers
{
    public class RegisterBookingDto
    {
        public int IdRoute { get; set; }
        public int IdVehicle { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumeber { get; set; }
        public required string Dni { get; set; }
        public string Email { get; set; } = string.Empty;
        public required string OperationCode { get; set; }
        public required string CardNumber { get; set; }
        public required string ExpirationDate { get; set; }
        public required string Cvv { get; set; }
        public required decimal FullPayment { get; set; }
        public required DateOnly TravelDate { get; set; }
        public required int[] Seats { get; set; }
    }
}
