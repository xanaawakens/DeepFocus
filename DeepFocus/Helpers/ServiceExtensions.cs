using Microsoft.Extensions.DependencyInjection;
using DeepFocus.Services;
using DeepFocus.ViewModels;
using DeepFocus.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Windows.Storage;

namespace DeepFocus.Helpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDeepFocusServices(this IServiceCollection services)
        {
            // Register DbContext
            var dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pomodoro.db");
            services.AddDbContext<PomodoroDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Register Services
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Register ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<SettingsViewModel>();

            return services;
        }
    }
}
