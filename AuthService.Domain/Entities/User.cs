using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entities
{
    public class ApplicationUser : BaseEntities
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? OtherName { get; set; }
        public string UserName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
