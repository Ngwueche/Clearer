

using AuthService.Domain.Entities;

namespace AuthService.Application.RepositoryInterfaces
{
    public interface IApplicationUserRepository
    {
        Task CreateUser(ApplicationUser user);
        Task<string> DeactivateUserAccount(Guid id);
        Task<ApplicationUser> GetUserById(Guid id, bool trackStatus);
        Task<ApplicationUser> GetActiveUserByUsernameAsync(string userName, bool trackStatus);
        Task UpdateUser(ApplicationUser adminRequest);
    }
}