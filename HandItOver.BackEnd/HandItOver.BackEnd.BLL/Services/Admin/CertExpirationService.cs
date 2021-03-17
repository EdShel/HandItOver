using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public class CertExpirationService
    {
        public Task<DateTime?> GetCertExpirationDateAsync(string url)
        {
            return new CertExpirationChecker().GetCertExpirationDateAsync(url);
        }
    }
}
