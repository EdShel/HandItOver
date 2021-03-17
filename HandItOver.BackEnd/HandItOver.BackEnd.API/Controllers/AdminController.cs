using HandItOver.BackEnd.API.Models.Admin;
using HandItOver.BackEnd.BLL.Services.Admin;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = AuthConstants.Roles.ADMIN)]
    public class AdminController : ControllerBase
    {
        private readonly DatabaseBackupService databaseBackupService;

        private readonly CertExpirationService certExpirationService;

        public AdminController(DatabaseBackupService databaseBackupService, CertExpirationService certExpirationService)
        {
            this.databaseBackupService = databaseBackupService;
            this.certExpirationService = certExpirationService;
        }

        [HttpPost("backup")]
        public async Task<IActionResult> BackupDatabaseAsync([FromBody] BackupDatabaseModel model)
        {
            await this.databaseBackupService.BackupDatabaseAsync(model.BackupFile);

            return Ok();
        }

        [HttpGet("backup")]
        public IActionResult SendBackup([FromQuery, Required] string file)
        {
            System.IO.Stream backupFileStream = this.databaseBackupService.GetBackupAsStream(file);
            return File(backupFileStream, "application/octet-stream");
        }

        [HttpGet("sslExpiration")]
        public async Task<IActionResult> GetSslExpirationDate()
        {
            string url = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/healthCheck";
            return new JsonResult(new
            {
                Expires = await this.certExpirationService.GetCertExpirationDateAsync(url)
            });
        }

        //[HttpGet("config")]
        //public IActionResult GetConfiguration()
        //{
        //    return Ok(configChangeService.GetConfigurations());
        //}

        //[HttpPut("config")]
        //public IActionResult SetConfiguration([FromBody] ConfigurationChangeRequest request)
        //{
        //    configChangeService.ChangeConfiguration(request.Select(opt => new ConfigurationOption
        //    {
        //        Key = opt.Key,
        //        Value = opt.Value
        //    }));
        //    return Ok();
        //}

        //[HttpGet("users")]
        //public async Task<IActionResult> GetUsersAsync(int pageIndex, int pageSize, string name)
        //{
        //    AppUsersPaginatedDTO result = await userService.GetUsersByNamePaginatedAsync(pageIndex, pageSize, name);
        //    return Ok(result);
        //}
    }
}
