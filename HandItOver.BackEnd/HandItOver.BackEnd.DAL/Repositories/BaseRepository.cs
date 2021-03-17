using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly DbContext dbContext;

        protected BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task SaveChangesAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}
