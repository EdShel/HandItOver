using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace HandItOver.BackEnd.DAL
{
    public static class DbSeeder
    {
        private const string DEFAULT_ROLE_ID = "99DA7670-5471-414F-834E-9B3A6B6C8F6F";
        private const string ADMIN_ROLE_ID = "2AEFE1C5-C5F0-4399-8FB8-420813567554";

        public static void SeedEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = DEFAULT_ROLE_ID,
                Name = AuthConstants.Roles.DEFAULT,
                NormalizedName = AuthConstants.Roles.DEFAULT.ToUpper()
            });

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = ADMIN_ROLE_ID,
                Name = AuthConstants.Roles.ADMIN,
                NormalizedName = AuthConstants.Roles.ADMIN.ToUpper()
            });
        }
    }
}
