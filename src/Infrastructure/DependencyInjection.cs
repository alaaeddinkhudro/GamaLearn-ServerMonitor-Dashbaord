using Application.Interfaces;
using Infrastructure.BackgroundJobs;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Seed;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<DbSeeder>();
        services.AddScoped<IServerRepository, ServerRepository>();
        services.AddScoped<IServerExistsRepository, ServerRepository>();
        services.AddScoped<IMetricsReadRepository, MetricsReadRepository>();
        services.AddScoped<IMetricRepository, MetricRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();
        services.AddScoped<MetricsSimulationJob>();

        return services;
    }
}
