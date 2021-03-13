using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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

            var authSettingsSection = configuration.GetSection(nameof(AuthSettings));
            var authSettings = authSettingsSection.Get<AuthSettings>();
            services.AddSingleton<AuthSettings>(authSettings);
            var jwtSigningKey = Encoding.UTF8.GetBytes(authSettings.SigningKey);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(
                authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
                configureOptions: options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtSigningKey),
                        ValidIssuer = authSettings.ValidIssuer,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                }
            );

            services.AddSingleton<IAuthTokenFactory, JwtTokenGenerator>();
            services.AddSingleton<IRefreshTokenFactory, RefreshTokenFactory>();
            services.AddScoped<UserRepository>();
            services.AddScoped<UsersService>();
            services.AddScoped<AuthService>();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {

            return services;
        }
    }
}
