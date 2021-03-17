using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories.Admin
{
    public class DatabaseRepository : BaseRepository
    {
        public DatabaseRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task MakeBackupAsync(string backupFile)
        {
            return this.dbContext.Database.ExecuteSqlRawAsync(
                sql: "BACKUP DATABASE {0} TO DISK = {1}",
                parameters: new object[] { this.dbContext.Database.GetDbConnection().Database, backupFile }
            );
        }
    }
}
