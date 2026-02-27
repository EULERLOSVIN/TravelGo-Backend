using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices
{
    public class ReniecResponse
    {
        public string dni { get; set; } = string.Empty;
        public string nombres { get; set; } = string.Empty;
        public string apellidoPaterno { get; set; } = string.Empty;
        public string apellidoMaterno { get; set; } = string.Empty;
    }

    public class ApisPeruService : IReniecService 
    {
        private readonly HttpClient _http;
        private readonly string _token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6ImV1bGVybG9zdmlubWFydGluZXpodXJ0YWRvQGdtYWlsLmNvbSJ9.xra7yyRSK4iGqB_Xa-7EMXI1DNF_Qia364PBS_z9UoI";

        public ApisPeruService(HttpClient http) => _http = http;

        public async Task<PersonApiDto?> ConsultByDni(int dni)
        {
            try
            {
                var url = $"dni/{dni}?token={_token}";

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var data = await response.Content.ReadFromJsonAsync<ReniecResponse>();

                if (data == null || string.IsNullOrEmpty(data.nombres))
                {
                    return null;
                }

                // Retornamos directamente el DTO con el nombre formateado
                return new PersonApiDto
                {
                    Dni = data.dni,
                    FullName = $"{data.nombres} {data.apellidoPaterno} {data.apellidoMaterno}"
                };
            }
            catch
            {
                return null;
            }
        }
    }
}