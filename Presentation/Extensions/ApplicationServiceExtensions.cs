using Application.Data;
using Application.Respository;
using Application.Services;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Infrastructure.DataContext;
using Application.Helpers;

namespace Presentation.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static readonly ILoggerFactory dbLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
           // services.Configure<Jwt>(config.GetSection("Jwt"));
       //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       //.AddJwtBearer(options =>
       //{
       //    options.TokenValidationParameters = new TokenValidationParameters
       //    {
       //        ValidateIssuer = true,
       //        ValidateAudience = true,
       //        ValidateLifetime = true,
       //        ValidateIssuerSigningKey = true,
       //        ValidIssuer = config["Jwt:Issuer"],
       //        ValidAudience = config["Jwt:Issuer"],
       //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
       //    };
       //});

            services.AddFluentValidation(x =>
            {
                x.DisableDataAnnotationsValidation = true;
                //x.RegisterValidatorsFromAssemblyContaining<>();
            });

            #region Redis Dependencies

            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            #endregion

            services.AddPooledDbContextFactory<AppDbContext>(option => option.UseLoggerFactory(dbLoggerFactory).UseSqlServer(config.GetConnectionString("DefualtConnection")));
            services.AddTransient<AppDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IOnboarding, OnboardingService>();
            services.AddScoped<IRedisCache, RedisCache>();
            services.AddScoped<IRedisDataContext, RedisDataContext>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            return services;
        }
    }
}
