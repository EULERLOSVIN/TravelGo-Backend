// rutas=darwin Este DTO sirve para RECIBIR los datos cuando creamos una ruta nueva.
namespace Application.DTOs
{
    public class AddTravelRouteDto
    {
        public required string nameRoute { get; set; }
        public decimal? price { get; set; }
        public int idPlaceA { get; set; }
        public int idPlaceB { get; set; }
    }
}
