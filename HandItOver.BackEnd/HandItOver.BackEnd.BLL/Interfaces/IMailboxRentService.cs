using HandItOver.BackEnd.BLL.Models.MailboxRent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxRentService
    {
        Task CancelRent(string rentId);
        Task<RentResult> GetRent(string rentId);
        Task<IEnumerable<RentResult>> GetRentsOfUserAsync(string userId);
        Task<RentResult> RentMailbox(RentRequest request);
    }
}