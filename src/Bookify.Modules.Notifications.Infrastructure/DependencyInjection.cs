using Bookify.Modules.Notifications.Application.Abstract;
using Bookify.Modules.Notifications.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Modules.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static void AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
    }
}