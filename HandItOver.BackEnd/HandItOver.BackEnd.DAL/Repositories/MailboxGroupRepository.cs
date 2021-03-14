using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
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

        public Task<MailboxGroup?> GetWhitelistByIdAsync(string id)
        {
            return this.dbContext.Set<MailboxGroup?>()
                .Include(m => m.Whitelisted)
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
}
