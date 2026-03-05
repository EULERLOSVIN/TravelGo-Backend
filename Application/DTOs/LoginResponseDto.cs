public class LoginResponseDto
{
    // Agregado para que módulos independientes (como Mi Perfil) puedan identificar al usuario logueado
    public int IdAccount { get; set; }
    
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}