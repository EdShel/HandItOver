using HandItOver.BackEnd.DAL.Repositories.Admin;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Admin;
using System.IO;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public class DatabaseBackupService
    {
        private readonly BackupSettings settings;

        private readonly DatabaseRepository database;

        public DatabaseBackupService(
            BackupSettings settings,
            DatabaseRepository database)
        {
            this.settings = settings;
            this.database = database;
        }

        public async Task BackupDatabaseAsync(string backupFile)
        {
            string pathToBackup = Path.Combine(this.settings.BackupPath, backupFile);
            string? backupDirectory = Path.GetDirectoryName(pathToBackup);
            if (backupDirectory == null)
            {
                throw new WrongValueException("Backup directory");
            }
            Directory.CreateDirectory(backupDirectory);

            await this.database.MakeBackupAsync(pathToBackup);
        }

        public async Task RestoreDatabaseAsync(string backupFile)
        {
            string pathToBackup = Path.Combine(this.settings.BackupPath, backupFile);
            string? backupDirectory = Path.GetDirectoryName(pathToBackup);
            if (backupDirectory == null)
            {
                throw new WrongValueException("Backup directory");
            }

            await this.database.RestoreBackupAsync(pathToBackup);
        }

        public Stream GetBackupAsStream(string backupFile)
        {
            string pathToBackup = Path.Combine(this.settings.BackupPath, backupFile);
            if (!File.Exists(pathToBackup))
            {
                throw new NotFoundException("Backup file");
            }
            return new FileStream(pathToBackup, FileMode.Open, FileAccess.Read);
        }
    }
}
