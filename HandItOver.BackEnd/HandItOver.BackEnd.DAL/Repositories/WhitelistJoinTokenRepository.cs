using HandItOver.BackEnd.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class WhitelistJoinTokenRepository : BaseRepository
    {
        public WhitelistJoinTokenRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<WhitelistJoinToken>> GetTokensOfGroup(string groupId)
        {
            return await this.dbContext.Set<WhitelistJoinToken>()
                .Where(t => t.GroupId == groupId)
                .ToListAsync();
        }

        public Task<WhitelistJoinToken> FindByIdOrNull(string tokenId)
        {
            return this.dbContext.Set<WhitelistJoinToken>()
                .FirstOrDefaultAsync(t => t.Id == tokenId);
        }

        public Task<WhitelistJoinToken> FindByGroupAndValueOrNull(string groupId, string value)
        {
            return this.dbContext.Set<WhitelistJoinToken>()
                .FirstOrDefaultAsync(t => t.GroupId == groupId && t.Token == value);
        }

        public void AddToken(WhitelistJoinToken token)
        {
            this.dbContext.Set<WhitelistJoinToken>().Add(token);
        }

        public void DeleteToken(WhitelistJoinToken token)
        {
            this.dbContext.Set<WhitelistJoinToken>().Remove(token);
        }
    }
}
