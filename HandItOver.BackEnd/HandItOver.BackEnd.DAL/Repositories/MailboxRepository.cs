using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public Task<MailboxRent?> FindByIdOrNull(string rentId)
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

    public class DeliveryRepository : BaseRepository<Delivery>
    {
        public DeliveryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public void AddDelivery(Delivery delivery)
        {
            this.dbContext.Set<Delivery>().Add(delivery);
        }

        public void UpdateDelivery(Delivery delivery)
        {
            this.dbContext.Set<Delivery>().Update(delivery);
        }

        public Task<Delivery?> GetCurrentDeliveryAsync(string mailboxId)
        {
            return this.dbContext.Set<Delivery?>()
                .FirstOrDefaultAsync(
                    d => d!.MailboxId == mailboxId && d.Taken == null
                );
        }
    }

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

    }
}
