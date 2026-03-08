using System;

namespace Application.DTOs.DepartureTimes
{
    public class DepartureTimeDto
    {
        public int IdDepartureTime { get; set; }
        public int IdTravelRoute { get; set; }
        public string Hour { get; set; } = string.Empty; // Devuelto como "HH:mm:ss"
    }

    public class AddDepartureTimeDto
    {
        public int IdTravelRoute { get; set; }
        public string Hour { get; set; } = string.Empty; // Recibido como "HH:mm:ss" desde el frontend
    }
}

