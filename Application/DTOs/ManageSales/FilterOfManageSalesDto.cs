

namespace Application.DTOs.ManageSales
{
    public class FilterOfManageSalesDto
    {
        public DateOnly? FromDate { get; set; }
        public DateOnly? UntilDate { get; set; }
        public int? IdRoute { get; set; }
        public int? StateTicket { get; set; }
        public int? page { get; set; }

    }
}
