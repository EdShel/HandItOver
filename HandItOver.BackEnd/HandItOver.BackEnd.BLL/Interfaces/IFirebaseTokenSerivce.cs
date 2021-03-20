using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IFirebaseTokenSerivce
    {
        Task RegisterFirebaseToken(string userId, string tokenValue);
    }
}