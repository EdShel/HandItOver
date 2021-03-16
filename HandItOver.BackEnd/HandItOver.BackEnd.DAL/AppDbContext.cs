using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public AppDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SeedEntities();

            builder.Entity<AppUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<AppRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<RefreshToken>(b =>
            {
                b.Property(t => t.Id).ValueGeneratedOnAdd();
                b.HasKey(t => t.Id);

                b.HasOne(t => t.AppUser)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(t => t.AppUserId)
                    .IsRequired();
            });

            builder.Entity<Mailbox>(b =>
            {
                b.Property(mailbox => mailbox.Id).ValueGeneratedOnAdd();

                b.HasOne(mailbox => mailbox.Owner)
                    .WithMany(user => user.Mailboxes)
                    .HasForeignKey(mailbox => mailbox.OwnerId);

                b.HasOne(mailbox => mailbox.MailboxGroup)
                    .WithMany(group => group!.Mailboxes)
                    .HasForeignKey(mailbox => mailbox.GroupId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<MailboxGroup>(b =>
            {
                b.HasKey(group => group.GroupId);
                b.Property(group => group.GroupId).ValueGeneratedOnAdd();

                b.HasOne(group => group.Owner)
                    .WithMany(user => user.OwnedGroups)
                    .HasForeignKey(group => group.OwnerId)
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasMany(group => group.Whitelisted)
                    .WithMany(user => user.WhitelistedIn);
            });

            builder.Entity<MailboxRent>(b =>
            {
                b.HasKey(rent => rent.RentId);
                b.Property(rent => rent.RentId).ValueGeneratedOnAdd();

                b.HasOne(rent => rent.Mailbox)
                    .WithMany(mailbox => mailbox.Rents)
                    .HasForeignKey(rent => rent.MailboxId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(rent => rent.Renter)
                    .WithMany(user => user.RentedMailboxes)
                    .HasForeignKey(rent => rent.RenterId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Delivery>(b =>
            {
                b.Property(d => d.Id).ValueGeneratedOnAdd();

                b.HasOne(d => d.Addressee)
                    .WithMany(user => user.Deliveries)
                    .HasForeignKey(d => d.AddresseeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
