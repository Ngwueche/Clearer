using System.Linq.Expressions;
using AuthService.Application.RepositoryInterfaces;
using AuthService.Data.EFCore;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.ServiceImplementations;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.RepositoryImplementations
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CreateUser(ApplicationUser adminRequest) => Create(adminRequest);
        public void UpdateUser(ApplicationUser adminRequest) => Update(adminRequest);

        public async Task<ApplicationUser> GetUserById(Guid id, bool trackStatus) => await FindByCondition(x => x.Id == id, trackStatus).Include(x => x.Role).FirstOrDefaultAsync();
        public async Task<ApplicationUser> GetUserByUsernameAsync(string userName, bool trackStatus) => await FindByCondition(x => x.UserName == userName, trackStatus).Include(x => x.Role).FirstOrDefaultAsync();

        public async Task<string> DeactivateUserAccount(Guid id)
        {
            var user = await GetUserById(id, true);
            if (user == null)
            {
                return "44";
            }
            user.IsActive = false;
            return "20";
        }

    }
}
