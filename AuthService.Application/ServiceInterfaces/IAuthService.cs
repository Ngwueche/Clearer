using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> DeactivateUserAsync(string username);
        Task<ApiResponse> DeleteUserAsync(string username);
        Task<ApiResponse> LoginAsync(string username, string password);
        Task<ApiResponse> LogOutAsync(string username);
        Task<ApiResponse> RefreshTokenAsync(string username, string refreshToken);
        Task<ApiResponse> RegisterAsync(RegisterUserDto dto);
        Task<ApiResponse> GetRolesAsync();
    }
}
