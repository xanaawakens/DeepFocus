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

        private bool _isLoading;
        private StatisticsSummary? _summary;
        private WeeklyStatistics? _weeklyStats;
        private ObservableCollection<DailyStatistics> _dailyStats = new();

        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        public StatisticsSummary? Summary
        {
            get => _summary;
            private set => SetProperty(ref _summary, value);
        }

        public WeeklyStatistics? WeeklyStats
        {
            get => _weeklyStats;
            private set => SetProperty(ref _weeklyStats, value);
        }

        public ObservableCollection<DailyStatistics> DailyStats
        {
            get => _dailyStats;
            private set => SetProperty(ref _dailyStats, value);
        }

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

        public async Task LoadStatisticsAsync()
        {
            try
            {
                IsLoading = true;

                // Get current week's statistics
                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                WeeklyStats = await _statisticsService.GetWeeklyStatisticsAsync(weekStart);

                // Get daily statistics for the last 7 days
                var dailyStatsList = await _statisticsService.GetDailyStatisticsAsync(
                    today.AddDays(-6), today);
                
                DailyStats.Clear();
                foreach (var stat in dailyStatsList.OrderBy(s => s.Date))
                {
                    DailyStats.Add(stat);
                }

                // Get overall summary
                Summary = await _statisticsService.GetStatisticsSummaryAsync();
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
