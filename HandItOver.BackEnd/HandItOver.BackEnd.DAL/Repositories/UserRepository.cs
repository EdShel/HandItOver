using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class MailboxRepository : BaseRepository<MailboxRepository>
    {
        public MailboxRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<Mailbox?> FindByPhysicalIdOrNullAsync(string physicalId)
        {
            return this.dbContext.Set<Mailbox?>()
                .FirstOrDefaultAsync(mb => mb!.PhysicalId == physicalId);
        }

        public Task<Mailbox?> FindByIdOrNullAsync(string id)
        {
            return this.dbContext.Set<Mailbox?>()
                .FirstOrDefaultAsync(mb => mb!.Id == id);
        }

        public void CreateMailbox(Mailbox mailbox)
        {
            this.dbContext.Set<Mailbox>().Add(mailbox);
        }
    }

    public class MailboxGroupRepository : BaseRepository<MailboxGroupRepository>
    {
        public MailboxGroupRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<MailboxGroup?> FindByNameOrNullAsync(string name)
        {
            return this.dbContext.Set<MailboxGroup?>()
                .Include(m => m!.Mailboxes)
                .FirstOrDefaultAsync(m => m!.Name == name);
        }

        public Task<MailboxGroup?> FindByIdOrNullAsync(string id)
        {
            return this.dbContext.Set<MailboxGroup?>()
                .Include(m => m!.Mailboxes)
                .FirstOrDefaultAsync(m => m!.GroupId == id);
        }

        public void CreateMailboxGroup(MailboxGroup mailboxGroup)
        {
            this.dbContext.Set<MailboxGroup>().Add(mailboxGroup);
        }

        public void DeleteMailboxGroup(MailboxGroup mailboxGroup)
        {
            this.dbContext.Set<MailboxGroup>().Remove(mailboxGroup);
        }
    }

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

        public Task<RefreshToken?> GetRefreshToken(AppUser user, string refreshToken)
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
    }
}
