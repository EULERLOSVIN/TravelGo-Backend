using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Persistence.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LoginRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var account = await _context.Accounts
                .Include(a => a.IdPersonNavigation)
                .Include(a => a.IdRoleNavigation)
                .Include(a => a.IdStateAccountNavigation)
                .FirstOrDefaultAsync(x => x.Email == loginRequestDto.Email);

            if (account == null) return null;

            if (account.IdStateAccountNavigation?.Name != "Activo") return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, account.Password);
            if (!isValid) return null;

            string token = GenerateToken(account);

            return new LoginResponseDto
            {
                Token = token,
                Email = account.Email,
                Rol = account.IdRoleNavigation?.Name ?? "Sin Rol",
                IdAccount = account.IdAccount
            };
        }

        private string GenerateToken(Account account)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.IdAccount.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("Nombre", account.IdPersonNavigation?.FirstName ?? "Usuario"),
                new Claim("Rol", account.IdRoleNavigation?.Name ?? "User")
            };

            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada en appsettings.json");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}