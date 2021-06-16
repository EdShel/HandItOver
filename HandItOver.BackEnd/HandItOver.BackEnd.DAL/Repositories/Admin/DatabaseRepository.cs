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

        public Task RestoreBackupAsync(string backupFile)
        {
            return this.dbContext.Database.ExecuteSqlRawAsync(
                sql: "USE [master]; RESTORE DATABASE {0} FROM DISK = {1} WITH REPLACE",
                parameters: new object[] {this.dbContext.Database.GetDbConnection().Database, backupFile }
            );
        }
    }
}
