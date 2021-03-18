using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class FirebaseTokenRepository : BaseRepository
    {
        public FirebaseTokenRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<FirebaseToken?> FindTokenForUserOrNull(string userId)
        {
            return this.dbContext.Set<FirebaseToken?>()
                .FirstOrDefaultAsync(t => t!.UserId == userId);
        }

        public void AddToken(FirebaseToken token)
        {
            this.dbContext.Set<FirebaseToken>().Add(token);
        }

        public void UpdateToken(FirebaseToken token)
        {
            this.dbContext.Set<FirebaseToken>().Update(token);
        }
    }
}
