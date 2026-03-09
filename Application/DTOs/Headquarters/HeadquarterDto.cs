using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Headquarters
{
    public class HeadquarterDto
    {
        public int IdHeadquarter { get; set; }
        public string Name { get; set; }= null!;
        public string Address { get; set; } =null!;
        public string Department { get; set; } =null!;
        public string Province { get; set; } =null!;
        public string District { get; set; } =null!;
        public string Phone { get; set; } =null!;
        public string? Email { get; set; } 
        public bool IsMain { get; set; }
        public DateTime? RegistrationDate { get; set; }// Nuevo: Para VER cuándo se creó
        public string StateHeadquarter { get; set; } = null!;// Explicación: El frontend no sabe qué es el ID "1" o "2", necesita ver "Activo" o "Inactivo".
        // Por eso aquí devolvemos el STRING (nombre), no el INT (id).
        public bool HasRoutes { get; set; }

    }
}
