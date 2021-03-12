using HandItOver.BackEnd.DAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class UserRepository : BaseRepository<AppUser>
    {
        private readonly UserManager<AppUser?> userManager;

        public UserRepository(DbContext dbContext, UserManager<AppUser?> userManager) : base(dbContext)
        {
            this.userManager = userManager;
        }

        public Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public Task<AppUser?> FindByEmailOrNullAsync(string email)
        {
            return this.userManager.FindByEmailAsync(email);
        }

        public Task<AppUser?> FindByIdOrNull(string id)
        {
            return this.userManager.FindByIdAsync(id);
        }

        public Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return this.userManager.CheckPasswordAsync(user, password);
        }

        public void CreateRefreshToken(AppUser user, RefreshToken refreshToken)
        {
            user.RefreshTokens.Add(refreshToken);
            this.dbContext.Update(user);
        }
    }
}
