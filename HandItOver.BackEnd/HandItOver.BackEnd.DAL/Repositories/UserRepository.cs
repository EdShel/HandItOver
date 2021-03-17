using HandItOver.BackEnd.DAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class UserRepository : BaseRepository
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

        public Task<AppUser?> FindByIdOrNullAsync(string id)
        {
            return this.userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(AppUser user)
        {
            return await this.dbContext.Set<AppUserRole>()
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToArrayAsync();
        }

        public Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return this.userManager.CheckPasswordAsync(user, password);
        }

        public void CreateRefreshToken(AppUser user, RefreshToken refreshToken)
        {
            refreshToken.AppUserId = user.Id;
            this.dbContext.Set<RefreshToken>().Add(refreshToken);
        }

        public Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
        {
            return this.userManager.AddToRoleAsync(user, role);
        }

        public Task DeleteUserAsync(AppUser user)
        {
            return this.userManager.DeleteAsync(user);
        }

        public Task<RefreshToken?> GetRefreshTokenAsync(AppUser user, string refreshToken)
        {
            return this.dbContext.Set<RefreshToken?>()
                .FirstOrDefaultAsync(token => token!.AppUserId == user.Id && token.Value == refreshToken);
        }

        public void DeleteRefreshToken(RefreshToken refreshToken)
        {
            this.dbContext.Set<RefreshToken>().Remove(refreshToken);
        }

        public Task<bool> IsAnyoneOfRoleRegisteredAsync(string roleName)
        {
            return this.dbContext.Set<AppUserRole>()
                .AnyAsync(ur => ur.Role.Name == roleName);
        }

        public Task<AppUser[]> FindByNameOrEmailAsync(string searchParam)
        {
            return this.dbContext.Set<AppUser>()
                .Where(u => u.Email.Contains(searchParam)
                            || u.FullName.Contains(searchParam))
                .ToArrayAsync();
        }
    }
}
