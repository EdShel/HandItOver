﻿// <auto-generated />
using System;
using HandItOver.BackEnd.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HandItOver.BackEnd.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AppUserMailboxGroup", b =>
                {
                    b.Property<string>("WhitelistedId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WhitelistedInGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WhitelistedId", "WhitelistedInGroupId");

                    b.HasIndex("WhitelistedInGroupId");

                    b.ToTable("AppUserMailboxGroup");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                            ConcurrencyStamp = "d11f263a-501b-487b-8a9e-574e4306224b",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                            ConcurrencyStamp = "744d1b5e-6370-4876-a859-8f1a30904a63",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "CF87335D-19AB-4413-86C1-30BC80A741FA",
                            ConcurrencyStamp = "0f7e27c6-78ff-4528-bffa-7fecb230569d",
                            Name = "mailbox",
                            NormalizedName = "MAILBOX"
                        });
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Delivery", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddresseeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Arrived")
                        .HasColumnType("datetime2");

                    b.Property<string>("MailboxId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PredictedTakingTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Taken")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TerminalTime")
                        .HasColumnType("datetime2");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AddresseeId");

                    b.HasIndex("MailboxId");

                    b.ToTable("Delivery");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Mailbox", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhysicalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Mailbox");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.MailboxGroup", b =>
                {
                    b.Property<string>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("MaxRentTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("WhitelistOnly")
                        .HasColumnType("bit");

                    b.HasKey("GroupId");

                    b.HasIndex("OwnerId");

                    b.ToTable("MailboxGroup");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.MailboxRent", b =>
                {
                    b.Property<string>("RentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<string>("MailboxId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RenterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Until")
                        .HasColumnType("datetime2");

                    b.HasKey("RentId");

                    b.HasIndex("MailboxId");

                    b.HasIndex("RenterId");

                    b.ToTable("MailboxRent");
                });

            modelBuilder.Entity("AppUserMailboxGroup", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("WhitelistedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HandItOver.BackEnd.DAL.Entities.MailboxGroup", null)
                        .WithMany()
                        .HasForeignKey("WhitelistedInGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppRoleClaim", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserClaim", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserLogin", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserRole", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUserToken", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.RefreshToken", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "AppUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Delivery", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "Addressee")
                        .WithMany("Deliveries")
                        .HasForeignKey("AddresseeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Mailbox", "Mailbox")
                        .WithMany("Deliveries")
                        .HasForeignKey("MailboxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Addressee");

                    b.Navigation("Mailbox");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Mailbox", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.MailboxGroup", "MailboxGroup")
                        .WithMany("Mailboxes")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "Owner")
                        .WithMany("Mailboxes")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MailboxGroup");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.MailboxGroup", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "Owner")
                        .WithMany("OwnedGroups")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.MailboxRent", b =>
                {
                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Mailbox", "Mailbox")
                        .WithMany("Rents")
                        .HasForeignKey("MailboxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", "Renter")
                        .WithMany("RentedMailboxes")
                        .HasForeignKey("RenterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Mailbox");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Auth.AppUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Deliveries");

                    b.Navigation("Logins");

                    b.Navigation("Mailboxes");

                    b.Navigation("OwnedGroups");

                    b.Navigation("RefreshTokens");

                    b.Navigation("RentedMailboxes");

                    b.Navigation("Tokens");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.Mailbox", b =>
                {
                    b.Navigation("Deliveries");

                    b.Navigation("Rents");
                });

            modelBuilder.Entity("HandItOver.BackEnd.DAL.Entities.MailboxGroup", b =>
                {
                    b.Navigation("Mailboxes");
                });
#pragma warning restore 612, 618
        }
    }
}
