using System.ComponentModel.DataAnnotations;

namespace HandItOver.BackEnd.API.Models.Admin
{
    public class BackupDatabaseModel
    {
        [Required]
        public string BackupFile { get; set; } = null!;
    }
}
