using HandItOver.BackEnd.BLL.Models.MailboxRent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxRentService
    {
        Task CancelRentAsync(string rentId);
        Task<RentResult> GetRentAsync(string rentId);
        Task<IEnumerable<RentResult>> GetRentsOfUserAsync(string userId);
        Task<RentResult> RentMailboxAsync(RentRequest request);
    }
}