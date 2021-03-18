using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HandItOver.BackEnd.API.Models.Firebase;
using HandItOver.BackEnd.BLL.ResourceAccess;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.BLL.Services.Admin;
using HandItOver.BackEnd.DAL;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.DAL.Repositories.Admin;
using HandItOver.BackEnd.Infrastructure.Models.Admin;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Principal;

namespace HandItOver.BackEnd.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.ClaimsIdentity.UserIdClaimType = AuthConstants.Claims.ID;
                options.ClaimsIdentity.EmailClaimType = AuthConstants.Claims.EMAIL;
                options.ClaimsIdentity.RoleClaimType = AuthConstants.Claims.ROLE;

                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<AppDbContext>();

            AddAuthTokenServices(services, configuration);

            AddRefreshTokenFactory(services, configuration);

            services.AddScoped<UserRepository>();
            services.AddScoped<UsersService>();
            services.AddScoped<AuthService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthConstants.Policies.MAILBOX_OWNER_ONLY,
                    policy => policy.Requirements.Add(MailboxOwnerAuthorizationHandler.GetRequirement("mailboxId"))
                );

                options.AddPolicy(
                    AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY,
                    policy => policy.Requirements.Add(MailboxGroupAuthorizationHandler.GetRequirement("groupId"))
                );

                options.AddPolicy(
                    AuthConstants.Policies.RENTER_OR_OWNER_ONLY,
                    policy => policy.Requirements.Add(RentAuthorizationHandler.GetRequirement("rentId"))
                );

                options.AddPolicy(
                    AuthConstants.Policies.DELIVERY_ADDRESSEE_ONLY,
                    policy => policy.Requirements.Add(DeliveryAuthorizationHandler.GetRequirement("deliveryId"))
                );
            });

            services.AddScoped<IAuthorizationHandler, MailboxOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MailboxGroupAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RentAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, DeliveryAuthorizationHandler>();

            services.AddSingleton<IClaimsTransformation, ClaimsTransformation>();

            return services;
        }

        private static void AddAuthTokenServices(IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettingsSection = configuration.GetSection(nameof(JwtTokenSettings));
            var tokenSettings = tokenSettingsSection.Get<JwtTokenSettings>();
            var tokenService = new JwtTokenService(tokenSettings);
            services.AddSingleton<ITokenService>(tokenService);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
                configureOptions: options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = tokenService.ValidationParameters;
                    options.MapInboundClaims = false;
                }
            );
        }

        private static void AddRefreshTokenFactory(IServiceCollection services, IConfiguration configuration)
        {
            var refreshTokenSettingsSection = configuration.GetSection(nameof(RefreshTokenSettings));
            var refreshTokenSettings = refreshTokenSettingsSection.Get<RefreshTokenSettings>();
            services.AddSingleton(refreshTokenSettings);
            services.AddSingleton<IRefreshTokenFactory, RefreshTokenFactory>();
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MailboxRepository>();
            services.AddScoped<MailboxGroupRepository>();
            services.AddScoped<WhitelistJoinTokenRepository>();
            services.AddScoped<RentRepository>();
            services.AddScoped<DeliveryRepository>();

            services.AddScoped<MailboxService>();
            services.AddScoped<MailboxAccessControlService>();
            services.AddScoped<MailboxGroupService>();
            services.AddScoped<MailboxRentService>();
            services.AddScoped<DeliveryService>();

            services.AddScoped(s =>
            {
                var dbBackupSection = configuration.GetSection(nameof(BackupSettings));
                var dbBackup = dbBackupSection.Get<BackupSettings>();
                return dbBackup;
            });
            services.AddScoped<DatabaseRepository>();
            services.AddScoped<DatabaseBackupService>();
            services.AddSingleton<CertExpirationService>();

            services.AddSingleton(s =>
            {
                var emailSettingsSection = configuration.GetSection("Email");
                var emailSettings = emailSettingsSection.Get<EmailOptions>();
                return emailSettings;
            });
            services.AddSingleton<EmailService>();

            services.AddSingleton(s => configuration.GetSection(nameof(SslMonitoringOptions)).Get<SslMonitoringOptions>());
            services.AddHostedService<CertExpirationNotifyService>();
            services.AddScoped<ConfigurationService>();

            var firebaseOptions = configuration.GetSection("Firebase").Get<FirebaseSettings>();
            var firebaseOptionsJson = JsonConvert.SerializeObject(firebaseOptions);
            var firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(firebaseOptionsJson)
            });
            var message = new Message
            {

            };
            FirebaseMessaging.DefaultInstance.
            return services;
        }
    }
}
