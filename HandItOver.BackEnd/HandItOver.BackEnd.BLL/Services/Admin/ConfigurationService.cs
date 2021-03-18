using HandItOver.BackEnd.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public class ConfigurationService
    {
        private readonly IHostEnvironment environment;

        private readonly ConfigurationRoot configuration;

        public ConfigurationService(
            IHostEnvironment environment,
            IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = (configuration as ConfigurationRoot)!;
        }

        public Task<string> GetConfigurationsAsync(string configFileName)
        {
            var path = GetConfigFilePath(configFileName);

            return File.ReadAllTextAsync(path);
        }

        public async Task UpdateConfigurationsAsync(string configFileName, string fileContent)
        {
            var path = GetConfigFilePath(configFileName);
            await File.WriteAllTextAsync(path, fileContent);
            this.configuration.Reload();
        }

        private string GetConfigFilePath(string configFileName)
        {
            var fileProvider = this.environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(configFileName);
            var physicalPath = fileInfo.PhysicalPath;

            if (!File.Exists(physicalPath))
            {
                throw new NotFoundException("Configuration file");
            }

            return physicalPath;
        }
    }
}
