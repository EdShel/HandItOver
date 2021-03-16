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
}
