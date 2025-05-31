

using AuthService.Domain.Entities;

namespace AuthService.Application.RepositoryInterfaces
{
    public interface IApplicationUserRepository
    {
        Task CreateUser(ApplicationUser user);
        Task<string> DeactivateUserAccount(Guid id);
        Task<string> DeleteUserAccount(Guid id);
        Task<ApplicationUser> GetUserById(Guid id, bool trackStatus);
        Task<ApplicationUser> GetUserByUsernameAsync(string userName, bool trackStatus);
        Task UpdateUser(ApplicationUser adminRequest);
    }
}