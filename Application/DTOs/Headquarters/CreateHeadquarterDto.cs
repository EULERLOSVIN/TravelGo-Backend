using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Application.DTOs.Headquarters
{
    public class CreateHeadquarterDto
    {
        [Required]// Valida que no venga nulo
        public int IdCompany { get; set; }
        [Required]
        public int IdStateHeadquarter { get; set; }
        [Required]
        public required string Name { get; set; } 
        public required string Address { get; set; } 
        public required string Department { get; set; } 
        public required string Province { get; set; } 
        public required string District { get; set; } 
        public required string Phone { get; set; } 
        public string? Email { get; set; } 
        public required bool IsMain { get; set; } // PascalCase

    }
}
