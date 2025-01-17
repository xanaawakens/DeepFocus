using System;
using System.Collections.Generic;

namespace DeepFocus.Models
{
    public class DailyStatistics
    {
        public DateTime Date { get; set; }
        public int CompletedPomodoros { get; set; }
        public int TotalMinutesFocused { get; set; }
        public List<PomodoroSession> Sessions { get; set; } = new();
    }

    public class WeeklyStatistics
    {
        public DateTime WeekStartDate { get; set; }
        public int TotalPomodoros { get; set; }
        public int TotalMinutesFocused { get; set; }
        public Dictionary<DayOfWeek, int> PomodorosByDay { get; set; } = new();
        public float AveragePomodorosPerDay => TotalPomodoros / 7.0f;
    }

    public class StatisticsSummary
    {
        public int TotalPomodoros { get; set; }
        public int TotalMinutesFocused { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime? MostProductiveDay { get; set; }
        public int MostPomodorosInDay { get; set; }
    }
}
