namespace HandItOver.BackEnd.Infrastructure.Services
{
    public interface IRefreshTokenFactory
    {
        public string GenerateRefreshToken();
    }
}
