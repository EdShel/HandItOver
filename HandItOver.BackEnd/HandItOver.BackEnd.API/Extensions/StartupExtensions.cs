using HandItOver.BackEnd.BLL.ResourceAccess;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<IAuthorizationHandler, MailboxGroupAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY,
                    policy => policy.Requirements.Add(new ResourceOwnerRequirement())
                );
            });

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
            }).AddJwtBearer(
                authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
                configureOptions: options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = tokenService.ValidationParameters;
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

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<MailboxRepository>();
            services.AddScoped<MailboxGroupRepository>();
            services.AddScoped<RentRepository>();
            services.AddScoped<DeliveryRepository>();

            services.AddScoped<MailboxService>();
            services.AddScoped<MailboxAccessControlService>();
            services.AddScoped<MailboxGroupService>();
            services.AddScoped<MailboxRentService>();
            services.AddScoped<DeliveryService>();

            return services;
        }
    }
}
