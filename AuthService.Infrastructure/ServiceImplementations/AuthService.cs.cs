using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.Interfaces;
using AuthService.Application.RepositoryInterfaces;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.ServiceImplementations
{
    public class AuthenticationService : IAuthService
    {
        private readonly IApplicationUserRepository _userRepository;

        public AuthenticationService(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> RegisterAsync(string username, string pAuthServiceword, Guid roleId)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username, false);
            if (existingUser != null)
                return "43";

            var user = new ApplicationUser
            {
                UserName = username,
                PAuthServicewordHash = HashPAuthServiceword(pAuthServiceword),
                RoleId = roleId
            };

            _userRepository.CreateUser(user);
            return "00";
        }

        public async Task<string> LoginAsync(string username, string pAuthServiceword)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username, false);
            if (user == null || !VerifyPAuthServiceword(pAuthServiceword, user.PAuthServicewordHash))
                return "40";

            return "00";
        }

        private string HashPAuthServiceword(string pAuthServiceword)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(pAuthServiceword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPAuthServiceword(string pAuthServiceword, string storedHash)
        {
            var hashed = HashPAuthServiceword(pAuthServiceword);
            return hashed == storedHash;
        }

    }
}
