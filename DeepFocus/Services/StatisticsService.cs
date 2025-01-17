using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeepFocus.Data;
using DeepFocus.Models;

namespace DeepFocus.Services
{
    public class StatisticsService : IStatisticsService, IDisposable
    {
        private readonly PomodoroDbContext _dbContext;

        public StatisticsService()
        {
            _dbContext = new PomodoroDbContext();
            _dbContext.Database.EnsureCreated();
        }

        public async Task<List<DailyStatistics>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var sessions = await _dbContext.Sessions
                .Where(s => s.StartTime.Date >= startDate.Date && s.StartTime.Date <= endDate.Date && s.IsCompleted)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            return sessions
                .GroupBy(s => s.StartTime.Date)
                .Select(g => new DailyStatistics
                {
                    Date = g.Key,
                    CompletedPomodoros = g.Count(s => s.SessionType == SessionType.Focus),
                    TotalMinutesFocused = g.Where(s => s.SessionType == SessionType.Focus)
                                         .Sum(s => s.Duration),
                    Sessions = g.ToList()
                })
                .OrderByDescending(d => d.Date)
                .ToList();
        }

        public async Task<WeeklyStatistics> GetWeeklyStatisticsAsync(DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.AddDays(6);
            var sessions = await _dbContext.Sessions
                .Where(s => s.StartTime.Date >= weekStartDate.Date && 
                           s.StartTime.Date <= weekEndDate.Date && 
                           s.IsCompleted &&
                           s.SessionType == SessionType.Focus)
                .ToListAsync();

            var pomodorosByDay = sessions
                .GroupBy(s => s.StartTime.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.Count());

            // Ensure all days of the week are represented
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (!pomodorosByDay.ContainsKey(day))
                {
                    pomodorosByDay[day] = 0;
                }
            }

            return new WeeklyStatistics
            {
                WeekStartDate = weekStartDate,
                TotalPomodoros = sessions.Count,
                TotalMinutesFocused = sessions.Sum(s => s.Duration),
                PomodorosByDay = pomodorosByDay
            };
        }

        public async Task<StatisticsSummary> GetStatisticsSummaryAsync()
        {
            var sessions = await _dbContext.Sessions
                .Where(s => s.IsCompleted && s.SessionType == SessionType.Focus)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            if (!sessions.Any())
            {
                return new StatisticsSummary();
            }

            var dailyPomodoros = sessions
                .GroupBy(s => s.StartTime.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostProductiveDay = dailyPomodoros
                .OrderByDescending(kvp => kvp.Value)
                .First();

            return new StatisticsSummary
            {
                TotalPomodoros = sessions.Count,
                TotalMinutesFocused = sessions.Sum(s => s.Duration),
                CurrentStreak = CalculateCurrentStreak(dailyPomodoros),
                LongestStreak = CalculateLongestStreak(dailyPomodoros),
                MostProductiveDay = mostProductiveDay.Key,
                MostPomodorosInDay = mostProductiveDay.Value
            };
        }

        public async Task AddSessionAsync(PomodoroSession session)
        {
            _dbContext.Sessions.Add(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetCurrentStreakAsync()
        {
            var sessions = await _dbContext.Sessions
                .Where(s => s.IsCompleted && s.SessionType == SessionType.Focus)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            var dailyPomodoros = sessions
                .GroupBy(s => s.StartTime.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            return CalculateCurrentStreak(dailyPomodoros);
        }

        private int CalculateCurrentStreak(Dictionary<DateTime, int> dailyPomodoros)
        {
            if (!dailyPomodoros.Any()) return 0;

            var currentDate = DateTime.Today;
            var streak = 0;

            while (dailyPomodoros.ContainsKey(currentDate))
            {
                streak++;
                currentDate = currentDate.AddDays(-1);
            }

            return streak;
        }

        private int CalculateLongestStreak(Dictionary<DateTime, int> dailyPomodoros)
        {
            if (!dailyPomodoros.Any()) return 0;

            var dates = dailyPomodoros.Keys.OrderBy(d => d).ToList();
            var currentStreak = 1;
            var longestStreak = 1;

            for (int i = 1; i < dates.Count; i++)
            {
                if (dates[i].AddDays(-1) == dates[i - 1])
                {
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else
                {
                    currentStreak = 1;
                }
            }

            return longestStreak;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
