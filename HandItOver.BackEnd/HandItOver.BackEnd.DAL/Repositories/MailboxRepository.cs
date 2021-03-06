using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class MailboxRepository : BaseRepository
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

        public async Task<IEnumerable<Mailbox>> FindByOwnerAsync(string userId)
        {
            return await this.dbContext.Set<Mailbox>()
                .Where(mb => mb.OwnerId == userId)
                .ToListAsync();
        }

        public Task<Mailbox?> FindByIdWithGroupOrNullAsync(string id)
        {
            return this.dbContext.Set<Mailbox?>()
                .Include(m => m!.MailboxGroup)
                .FirstOrDefaultAsync(mbox => mbox!.Id == id);
        }

        public void CreateMailbox(Mailbox mailbox)
        {
            this.dbContext.Set<Mailbox>().Add(mailbox);
        }

        public void UpdateMailbox(Mailbox mailbox)
        {
            this.dbContext.Set<Mailbox>().Update(mailbox);
        }
    }
}
