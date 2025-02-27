using Application.Use_Cases.Role.CreateRole;
using Domain.Interfaces;
using FluentValidation;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Data;
using Infrastructure.Repositories;
using Infrastructure.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Use_Cases.Role.Validators;
using Application.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Use_Cases.Auth.Register;
using Application.Use_Cases.Auth.LogIn;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Use_Cases.Auth.RefreshToken;
using Application.Use_Cases.Auth.LogOut;
using Application.Use_Cases.Auth.ForgotPassword;
using Application.Use_Cases.Auth.SetNewPassword;

namespace WebAPI
{
    public static class ServiceExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);  
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
            services.AddScoped<IRegisterUseCase, RegisterUseCase>();
            services.AddScoped<ILogInUseCase, LogInUseCase>();
            services.AddValidatorsFromAssemblyContaining<RoleValidator>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IAuthSettings, AuthSettings>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<ILogOutUseCase, LogOutUseCase>();
            services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
            services.AddScoped<IEmailService, MailpitEmailService>();
            services.AddScoped<IPasswordResetTokenGenerator, PasswordResetTokenGenerator>();
            services.AddScoped<ISetNewPasswordUseCase, SetNewPasswordUseCase>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RoleProfile).Assembly);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"]
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                    policy.RequireAuthenticatedUser());

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("admin"));

                options.AddPolicy("UserOnly", policy =>
                    policy.RequireRole("user"));
            });

        }

    }
}
