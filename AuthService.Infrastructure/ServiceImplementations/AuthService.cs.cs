using System;
using System.Threading.Tasks;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.RepositoryInterfaces;
using AuthService.Data.EFCore;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Infrastructure.ServiceImplementations
{
    public class AuthenticationService : IAuthService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IMemoryCache _cache;

        private const string USER_CACHE_KEY_PREFIX = "user_";

        public AuthenticationService(IApplicationUserRepository userRepository, ApplicationDbContext dbContext, IJwtService jwtService, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _jwtService = jwtService;
            _cache = cache;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterUserDto dto)
        {
            try
            {
                var existingUser = await _userRepository.GetActiveUserByUsernameAsync(dto.UserName, true);
                if (existingUser != null)
                {
                    return ApiResponseExtensions.Fail("43", $"User with username {dto.UserName} already exists.");
                }
                var validateRole = _dbContext.Roles.Any(x => x.Id == dto.RoleId);
                if (!validateRole)
                {
                    return ApiResponseExtensions.Fail("43", $"Role does not exist.");
                }

                var user = new ApplicationUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    OtherName = dto.OtherName,
                    UserName = dto.UserName,
                    PasswordHash = PasswordHelper.HashPassword(dto.Password),
                    RoleId = dto.RoleId,
                    IsActive = true
                };

                _userRepository.CreateUser(user);
                await _dbContext.SaveChangesAsync();
                return ApiResponseExtensions.Success("00", "User Created Successfully");
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        public async Task<ApiResponse> DeactivateUserAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return ApiResponseExtensions.Fail("40", $"Username cannot be null.");
                }
                var user = await _userRepository.GetActiveUserByUsernameAsync(username, false);
                if (user == null)
                {
                    return ApiResponseExtensions.Fail("43", $"User with username {username} does not exist.");
                }

                await _userRepository.DeactivateUserAccount(user.Id);
                _dbContext.SaveChanges();
                _cache.Remove($"{USER_CACHE_KEY_PREFIX}{username}");

                return ApiResponseExtensions.Success("00", "User deactivated successfully");
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        public async Task<ApiResponse> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userRepository.GetActiveUserByUsernameAsync(username, false);
                if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
                {
                    return ApiResponseExtensions.Fail("40", "Invalid username and password combination");
                }

                string refreshToken = user.RefreshToken;
                if (refreshToken is null || user.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    refreshToken = Guid.CreateVersion7().ToString();
                    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(3);
                    user.RefreshToken = PasswordHelper.HashPassword(refreshToken);
                }

                user.IsLoggedIn = true;

                _userRepository.UpdateUser(user);

                var jwtDto = new JwtDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    OtherName = user.OtherName,
                    Role = user.Role.RoleName,
                    UserName = user.UserName,
                    Id = user.Id
                };

                var loginDto = LoginDtoBuilder(jwtDto, refreshToken);
                AddUserToCache(jwtDto, refreshToken);
                return ApiResponseExtensions.Success("00", "Success", loginDto);
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        public async Task<ApiResponse> LogOutAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return ApiResponseExtensions.Fail("40", $"Username cannot be null.");
                }
                var user = await _userRepository.GetActiveUserByUsernameAsync(username, false);
                if (user == null)
                {
                    return ApiResponseExtensions.Fail("40", $"User with username {username} not found.");
                }

                user.RefreshToken = null;
                user.IsLoggedIn = false;

                _dbContext.SaveChanges();

                return ApiResponseExtensions.Success("00", "Success");
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        public async Task<ApiResponse> RefreshTokenAsync(string username, string refreshToken)
        {
            try
            {
                var user = await GetUserDetailsAsync(username, refreshToken);
                if (user == null)
                {
                    return ApiResponseExtensions.Fail("40", $"User with username {username} not found.");
                }

                var jwtDto = new JwtDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    OtherName = user.OtherName,
                    Role = user.Role,
                    UserName = user.UserName,
                    Id = user.Id
                };

                var loginDto = LoginDtoBuilder(jwtDto, refreshToken);

                return ApiResponseExtensions.Success("00", "Success", loginDto);
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        public async Task<ApiResponse> GetRolesAsync()
        {
            try
            {
                var roles = await _dbContext.Roles.Select(a => new RoleDto
                {
                    Id = a.Id,
                    RoleName = a.RoleName,
                }).ToListAsync();

                if (!roles.Any())
                {
                    return ApiResponseExtensions.Fail("40", "No role found.");
                }

                return ApiResponseExtensions.Success("00", "Suucess", roles);
            }
            catch
            {
                return ApiResponseExtensions.Fail("99", "Service not available, try again later.");
            }
        }

        private LoginDto LoginDtoBuilder(JwtDto jwtDto, string refreshToken)
        {
            var token = _jwtService.GenerateToken(jwtDto);

            return new LoginDto
            {
                FullName = $"{jwtDto.FirstName} {jwtDto.LastName}",
                RefreshToken = refreshToken,
                Token = token,
                Role = jwtDto.Role
            };
        }

        private void AddUserToCache(JwtDto user, string refreshToken)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OtherName = user.OtherName,
                UserName = user.UserName,
                Role = user.Role
            };

            _cache.Set(refreshToken, userDto);
        }

        private async Task<UserDto> GetUserDetailsAsync(string username, string refreshToken = null)
        {
            try
            {
                string cacheKey = $"{USER_CACHE_KEY_PREFIX}{refreshToken}";

                if (_cache.TryGetValue(cacheKey, out UserDto cachedUser))
                    return cachedUser;

                var user = await _userRepository.GetActiveUserByUsernameAsync(username, false);
                if (user == null) return null;

                var userDto = new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    OtherName = user.OtherName,
                    Role = user.Role?.RoleName,
                    Id = user.Id,
                    UserName = user.UserName
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(7));

                _cache.Set(cacheKey, userDto, cacheOptions);

                return userDto;
            }
            catch
            {
                return null; // Fail silently here since it's a helper
            }
        }
    }

}
