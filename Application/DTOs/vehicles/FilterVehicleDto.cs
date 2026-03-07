using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.vehicles
{
    public class FilterVehicleDto
    {
        public string? SearchTerm { get; set; }
        public int? PageNumber { get; set; }
    }
}
