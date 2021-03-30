using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxRentService
    {
        Task CancelRentAsync(string rentId);
        Task<IEnumerable<TimeInterval>> FindNearestIntervalsToRent(RentTimeCheckRequest request);
        Task<RentResult> GetRentAsync(string rentId);
        Task<IEnumerable<RentResult>> GetRentsForMailboxGroupAsync(string groupId);
        Task<IEnumerable<RentResult>> GetRentsOfUserAsync(string userId);
        Task<RentResult> RentMailboxAsync(RentRequest request);
    }
}