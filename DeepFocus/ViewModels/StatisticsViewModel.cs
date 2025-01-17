using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DeepFocus.Models;
using DeepFocus.Services;

namespace DeepFocus.ViewModels
{
    public partial class StatisticsViewModel : ViewModelBase
    {
        private readonly IStatisticsService _statisticsService;

        [ObservableProperty]
        private StatisticsSummary? summary;

        [ObservableProperty]
        private WeeklyStatistics? weeklyStats;

        [ObservableProperty]
        private ObservableCollection<DailyStatistics> dailyStats = new();

        [ObservableProperty]
        private bool isLoading;

        public StatisticsViewModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            Title = "Statistics";
        }

        public override async void Initialize()
        {
            await LoadStatisticsAsync();
            base.Initialize();
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                IsLoading = true;

                // Get current week's statistics
                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                weeklyStats = await _statisticsService.GetWeeklyStatisticsAsync(weekStart);

                // Get daily statistics for the last 7 days
                var dailyStatsList = await _statisticsService.GetDailyStatisticsAsync(
                    today.AddDays(-6), today);
                
                DailyStats.Clear();
                foreach (var stat in dailyStatsList.OrderBy(s => s.Date))
                {
                    DailyStats.Add(stat);
                }

                // Get overall summary
                summary = await _statisticsService.GetStatisticsSummaryAsync();
            }
            catch (Exception ex)
            {
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error loading statistics: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
