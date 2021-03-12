using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace HandItOver.BackEnd.DAL.Entities.Auth
{
    public class AppUser : IdentityUser
    {
        public ICollection<AppUserClaim> Claims { get; set; } = null!;

        public ICollection<AppUserLogin> Logins { get; set; } = null!;

        public ICollection<AppUserToken> Tokens { get; set; } = null!;

        public ICollection<AppUserRole> UserRoles { get; set; } = null!;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }

    public class AppRole : IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = null!;

        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; } = null!;
    }

    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser User { get; set; } = null!;

        public virtual AppRole Role { get; set; } = null!;
    }

    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser User { get; set; } = null!;
    }

    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser User { get; set; } = null!;
    }

    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole Role { get; set; } = null!;
    }

    public class AppUserToken : IdentityUserToken<string>
    {
        public virtual AppUser User { get; set; } = null!;
    }

    public static class AppClaims
    {
        public const string EMAIL = "email";
    }

    public class RefreshToken
    {
        public string Id { get; set; }

        public string AppUserId { get; set; } = null!;

        public string Value { get; set; } = null!;

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= this.Expires;

        public AppUser AppUser { get; set; } = null!;
    }
}
