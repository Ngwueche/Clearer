using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? OtherName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? OtherName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }
}
