﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
    public class JwtDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? OtherName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

}
