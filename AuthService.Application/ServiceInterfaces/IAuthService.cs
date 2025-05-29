using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string username, string pAuthServiceword, Guid role);
        Task<string> LoginAsync(string username, string pAuthServiceword);
    }
}
