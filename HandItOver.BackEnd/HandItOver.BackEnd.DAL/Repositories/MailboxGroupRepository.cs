using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class MailboxGroupRepository : BaseRepository
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

        public Task<MailboxGroup?> FindByIdFullInfoAsync(string id)
        {
            return this.dbContext.Set<MailboxGroup?>()
                .Include(m => m!.Mailboxes)
                    .ThenInclude(m => m.Deliveries.Where(delivery => delivery.Taken == null))
                .Include(m => m!.Mailboxes)
                    .ThenInclude(m => m.Rents.Where(r => r.From <= DateTime.UtcNow && DateTime.UtcNow <= r.Until))
                        .ThenInclude(r => r.Renter)
                .FirstOrDefaultAsync(m => m!.GroupId == id);
        }

        public Task<MailboxGroup?> GetWhitelistByIdAsync(string id)
        {
            return this.dbContext.Set<MailboxGroup?>()
                .Include(m => m!.Whitelisted)
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

        public Task<Mailbox[]> MailboxesWithoutDelivery(string groupId)
        {
            return this.dbContext.Set<Mailbox>()
                .Where(mb => mb.GroupId == groupId && !mb.Deliveries.Any(d => d.Taken == null))
                .ToArrayAsync();
        }

        // All mailboxes that have no rents periods intersection [rentBegin, rentEnd)
        public Task<Mailbox[]> MailboxesWithoutRent(string groupId, DateTime rentBegin, DateTime rentEnd)
        {
            return this.dbContext.Set<Mailbox>()
                .Where(mb => mb.GroupId == groupId
                    && !mb.Rents.Any(rent => rentBegin <= rent.Until && rent.From < rentEnd))
                .ToArrayAsync();
        }

        public void ReplaceMailboxGroup(MailboxGroup group)
        {
            this.dbContext.Set<MailboxGroup>().Attach(group);
            this.dbContext.Entry(group).State = EntityState.Modified;
        }

        public Task<MailboxGroup[]> FindByNameOrAddressOrOwnerAsync(string searchParam)
        {
            return this.dbContext.Set<MailboxGroup>()
                .Include(group => group.Owner)
                .Where(group => group.Name.Contains(searchParam)
                                || group.Mailboxes.Any(mb => mb.Address.Contains(searchParam))
                                || group.Owner.FullName.Contains(searchParam)
                                || group.Owner.Email.Contains(searchParam))
                .ToArrayAsync();
        }
    }
}
