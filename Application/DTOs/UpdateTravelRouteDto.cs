// rutas=darwin
namespace Application.DTOs
{
    public class UpdateTravelRouteDto
    {
        public int idTravelRoute { get; set; }
        public decimal? price { get; set; }
        public int idPlaceA { get; set; }
        public int idPlaceB { get; set; }
    }
}
