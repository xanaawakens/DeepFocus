using Microsoft.Extensions.DependencyInjection;
using DeepFocus.Services;
using DeepFocus.ViewModels;

namespace DeepFocus.Helpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDeepFocusServices(this IServiceCollection services)
        {
            // Register Services
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INotificationService, NotificationService>();

            // Register ViewModels
            services.AddTransient<MainViewModel>();

            return services;
        }
    }
}
