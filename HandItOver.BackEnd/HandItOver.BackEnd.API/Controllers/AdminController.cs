using HandItOver.BackEnd.API.Models.Admin;
using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.BLL.Services.Admin;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
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

        private readonly UsersService usersService;

        private readonly ConfigurationService configurationService;

        public AdminController(
            DatabaseBackupService databaseBackupService,
            CertExpirationService certExpirationService,
            UsersService usersService,
            ConfigurationService configurationService)
        {
            this.databaseBackupService = databaseBackupService;
            this.certExpirationService = certExpirationService;
            this.usersService = usersService;
            this.configurationService = configurationService;
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
            Stream backupFileStream = this.databaseBackupService.GetBackupAsStream(file);
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

        [HttpGet("config/{configFile}")]
        public async Task<IActionResult> GetConfiguration([FromRoute] string configFile)
        {
            var config = await this.configurationService.GetConfigurationsAsync(configFile);
            return Ok(config);
        }

        [HttpPut("config/{configFile}")]
        public async Task<IActionResult> SetConfiguration([FromRoute] string configFile)
        {
            string content;
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }
            await this.configurationService.UpdateConfigurationsAsync(configFile, content);
            return Ok();
        }

        [HttpGet("users")]
        public async Task<IActionResult> SearchPaginatedAsync(
            [FromQuery, Required] int pageIndex,
            [FromQuery, Required] int pageSize,
            [FromQuery] string? search
        )
        {
            var request = new UsersPaginatedRequest(
                SearchQuery: search,
                PageIndex: pageIndex,
                PageSize: pageSize
            );
            var result = await this.usersService.GetUsersPaginated(request);
            return new JsonResult(result);
        }
    }
}
