using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories
{
    internal class RouteRepository
    {
        public int IdRoute { get; set; } // Primary Key

        public string Origin { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public decimal DistanceKm { get; set; }

        public TimeSpan EstimatedDuration { get; set; }

        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
