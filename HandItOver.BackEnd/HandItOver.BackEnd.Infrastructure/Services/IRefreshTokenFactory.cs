using HandItOver.BackEnd.Infrastructure.Models.Auth;

namespace HandItOver.BackEnd.Infrastructure.Services
{
    public interface IRefreshTokenFactory
    {
        public IRefreshToken GenerateRefreshToken();
    }
}
