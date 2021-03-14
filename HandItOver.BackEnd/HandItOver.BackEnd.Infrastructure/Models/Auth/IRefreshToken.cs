using System;

namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public interface IRefreshToken
    {
        string Value { get; }

        DateTime Expires { get; }
    }
}
