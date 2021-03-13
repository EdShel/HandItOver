using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.Users
{
    public record UserInfoResult(
        string Id,
        string Email, 
        string Role
    );
}
