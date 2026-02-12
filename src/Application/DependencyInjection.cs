using Application.Servers.Features.ServersCrud;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Application.Users.Features.Login.LoginService>();
        services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
        services.AddScoped<Application.Servers.Features.ServersCrud.ServersService>();
        return services;
    }
}
