using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
