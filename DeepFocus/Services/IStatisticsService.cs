using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepFocus.Models;

namespace DeepFocus.Services
{
    public interface IStatisticsService
    {
        Task<List<DailyStatistics>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<WeeklyStatistics> GetWeeklyStatisticsAsync(DateTime weekStartDate);
        Task<StatisticsSummary> GetStatisticsSummaryAsync();
        Task AddSessionAsync(PomodoroSession session);
        Task<int> GetCurrentStreakAsync();
    }
}
