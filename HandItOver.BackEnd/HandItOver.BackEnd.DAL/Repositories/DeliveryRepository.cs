using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class DeliveryRepository : BaseRepository
    {
        public DeliveryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<Delivery?> FindByIdOrNull(string id)
        {
            return this.dbContext.Set<Delivery?>()
                .FirstOrDefaultAsync(d => d!.Id == id);
        }

        public async Task<IEnumerable<Delivery>> GetActiveDeliveriesOfUserAsync(string userId)
        {
            return await this.dbContext.Set<Delivery>()
                .Where(d => d.AddresseeId == userId && d.Taken == null)
                .ToListAsync();
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
