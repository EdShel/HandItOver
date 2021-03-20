using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IFirebaseTokenSerivce
    {
        Task RegisterFirebaseTokenAsync(string userId, string tokenValue);
    }
}