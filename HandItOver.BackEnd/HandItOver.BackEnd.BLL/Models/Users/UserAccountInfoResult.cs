using System.Collections.Generic;

namespace HandItOver.BackEnd.BLL.Models.Users
{
    public record UserAccountInfoResult(
        string Id,
        string Email,
        string FullName,
        string Role
    );

    public record UserPublicInfoResult(
        string Id,
        string Email,
        string FullName
    );

    public record UsersPaginatedRequest(
        string? SearchQuery,
        int PageIndex,
        int PageSize
    );

    public record UsersPaginatedResult(
        int TotalPages,
        int PageIndex,
        int PageSize,
        IEnumerable<UserPublicInfoResult> Users
    );
}
