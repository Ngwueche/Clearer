using AuthService.Domain.Entities;

namespace AuthService.Application.RepositoryInterfaces
{
    public interface IApplicationUserRepository
    {
        void CreateUser(ApplicationUser user);
        Task<string> DeactivateUserAccount(Guid id);
        Task<ApplicationUser> GetUserById(Guid id, bool trackStatus);
        Task<ApplicationUser> GetUserByUsernameAsync(string userName, bool trackStatus);
        void UpdateUser(ApplicationUser adminRequest);
    }
}