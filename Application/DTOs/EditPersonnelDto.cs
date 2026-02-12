using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class EditPersonnelDto
    {
        public int IdAccount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IdTypeDocument { get; set; }
        public string NumberDocument { get; set; }
        public string PhoneNumber { get; set; }
        public int IdRole { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int IdStateOfAccount { get; set; }        
    }
}
