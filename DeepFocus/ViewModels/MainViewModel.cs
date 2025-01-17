using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepFocus.Models;
using DeepFocus.Services;

namespace DeepFocus.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITimerService _timerService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string timeDisplay = "25:00";

        [ObservableProperty]
        private bool isRunning;

        [ObservableProperty]
        private string cycleDisplay = "Pomodoro 1/4";

        [ObservableProperty]
        private PomodoroSession? currentSession;

        public MainViewModel(
            ITimerService timerService,
            ISettingsService settingsService,
            INotificationService notificationService)
        {
            _timerService = timerService;
            _settingsService = settingsService;
            _notificationService = notificationService;

            _timerService.TimerTick += OnTimerTick;
            _timerService.TimerCompleted += OnTimerCompleted;
        }

        [RelayCommand]
        private void StartTimer()
        {
            if (currentSession == null)
            {
                currentSession = new PomodoroSession(_settingsService.FocusDuration, SessionType.Focus);
            }
            _timerService.Start();
            IsRunning = true;
        }

        [RelayCommand]
        private void PauseTimer()
        {
            _timerService.Pause();
            IsRunning = false;
        }

        [RelayCommand]
        private void ResetTimer()
        {
            _timerService.Reset();
            IsRunning = false;
            currentSession = null;
            TimeDisplay = $"{_settingsService.FocusDuration}:00";
        }

        private void OnTimerTick(object? sender, TimeSpan remaining)
        {
            TimeDisplay = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }

        private void OnTimerCompleted(object? sender, EventArgs e)
        {
            if (currentSession != null)
            {
                currentSession.IsCompleted = true;
                currentSession.EndTime = DateTime.Now;
            }
            _notificationService.ShowNotification("Time's up!", "Take a break!");
        }
    }
}
