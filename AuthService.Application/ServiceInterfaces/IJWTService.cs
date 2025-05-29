
using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(JwtDto user);
    }
}
