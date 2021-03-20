using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class FirebaseTokenSerivce : IFirebaseTokenSerivce
    {
        private readonly FirebaseTokenRepository firebaseTokenRepository;

        public FirebaseTokenSerivce(FirebaseTokenRepository firebaseTokenRepository)
        {
            this.firebaseTokenRepository = firebaseTokenRepository;
        }

        public async Task RegisterFirebaseTokenAsync(string userId, string tokenValue)
        {
            var existingToken = await this.firebaseTokenRepository.FindTokenForUserOrNull(userId);
            if (existingToken != null)
            {
                existingToken.Token = tokenValue;
                this.firebaseTokenRepository.UpdateToken(existingToken);
            }
            else
            {
                var newToken = new FirebaseToken
                {
                    UserId = userId,
                    Token = tokenValue
                };
                this.firebaseTokenRepository.AddToken(newToken);
            }
            await this.firebaseTokenRepository.SaveChangesAsync();
        }
    }
}
