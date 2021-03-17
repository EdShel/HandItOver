using System;
using System.Net;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public class CertExpirationChecker
    {
        public async Task<DateTime?> GetCertExpirationDateAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            DateTime? expiration = default;
            request.ServerCertificateValidationCallback += (sender, cert, chain, errors) =>
            {
                expiration = cert != null 
                ? DateTime.Parse(cert.GetExpirationDateString())
                : null;
                return true;
            };
            try
            {
                using (_ = await request.GetResponseAsync())
                {
                    return expiration;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
