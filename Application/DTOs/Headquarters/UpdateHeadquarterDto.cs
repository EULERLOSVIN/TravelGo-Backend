using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Headquarters
{
    public class UpdateHeadquarterDto : CreateHeadquarterDto
    {
        // En update, el ID es obligatorio para saber CUAL editar
        public int IdHeadquarter { get; set; }

    }
}
