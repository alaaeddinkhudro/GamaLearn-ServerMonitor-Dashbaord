using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Application.Users.Features.Login.LoginService>();
        services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
        services.AddScoped<Application.Servers.Features.ListServers.ListServersService>();
        return services;
    }
}
