using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class DeliveryRepository : BaseRepository<Delivery>
    {
        public DeliveryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<Delivery?> FindByIdOrNull(string id)
        {
            return this.dbContext.Set<Delivery?>()
                .FirstOrDefaultAsync(d => d!.Id == id);
        }

        public void AddDelivery(Delivery delivery)
        {
            this.dbContext.Set<Delivery>().Add(delivery);
        }

        public void UpdateDelivery(Delivery delivery)
        {
            this.dbContext.Set<Delivery>().Update(delivery);
        }

        public Task<Delivery?> GetCurrentDeliveryOrNullAsync(string mailboxId)
        {
            return this.dbContext.Set<Delivery?>()
                .Include(d => d!.Mailbox)
                .FirstOrDefaultAsync(
                    d => d!.MailboxId == mailboxId && d.Taken == null
                );
        }
    }
}
