using HandItOver.BackEnd.BLL.Models.Notification;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Notification
{
    public class FirebaseNotificationService
    {
        private readonly FirebaseRepository firebaseRepository;

        private readonly FirebaseTokenRepository firebaseTokenRepository;

        public FirebaseNotificationService(FirebaseRepository firebaseRepository, FirebaseTokenRepository firebaseTokenRepository)
        {
            this.firebaseRepository = firebaseRepository;
            this.firebaseTokenRepository = firebaseTokenRepository;
        }

        public async Task SendAsync(NotificationMessage message)
        {
            FirebaseToken token = await this.firebaseTokenRepository.FindTokenForUserOrNull(message.ReceiverAddress)
                ?? throw new NotFoundException("User firebase token");
            await this.firebaseRepository.SendMessageAsync(token.Token, message.MessageKey, message.Data);
        }
    }
}
