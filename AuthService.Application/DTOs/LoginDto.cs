using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class LoginDto
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
    public class SignDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenDto
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}
