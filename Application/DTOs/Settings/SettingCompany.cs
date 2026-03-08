using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Settings
{
    public class SettingCompany
    {
        public int IdCompany { get; set; }
        public string? BusinessName { get; set; }
        public string? Ruc { get; set; }
        public string? FiscalAddress { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
