using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class RentRepository : BaseRepository<MailboxRent>
    {
        public RentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public void CreateRent(MailboxRent rent)
        {
            this.dbContext.Set<MailboxRent>().Add(rent);
        }

        public Task<MailboxRent?> FindByIdOrNullAsync(string rentId)
        {
            return this.dbContext.Set<MailboxRent?>()
                .FirstOrDefaultAsync(rent => rent!.RentId == rentId);
        }

        public Task<MailboxRent?> FindByIdWithMailboxOrNull(string rentId)
        {
            return this.dbContext.Set<MailboxRent?>()
                .Include(rent => rent!.Mailbox)
                .FirstOrDefaultAsync(rent => rent!.RentId == rentId);
        }

        public Task<MailboxRent?> FindForTimeOrNull(string mailboxId, DateTime time)
        {
            return this.dbContext.Set<MailboxRent?>()
                .FirstOrDefaultAsync(r => r!.From >= time && time <= r.Until);
        }

        public void DeleteRent(MailboxRent rent)
        {
            this.dbContext.Set<MailboxRent>().Remove(rent);
        }
    }
}
