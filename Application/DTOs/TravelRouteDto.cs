// rutas=darwin   Este DTO sirve para MOSTRAR los datos al frontend.
namespace Application.DTOs
{
    public class TravelRouteDto
    {
        public int idTravelRoute { get; set; }
        public string nameRoute { get; set; } = string.Empty; // Ej: "Lima - Arequipa"
        public decimal? price { get; set; }
        public int idPlaceA { get; set; }
        public int idPlaceB { get; set; }
    }
}
