using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepFocus.Models;
using DeepFocus.Services;

namespace DeepFocus.ViewModels
{
    public partial class MainViewModel : ViewModelBase
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
        private double progress;

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

            Title = "Timer";
            Initialize();
        }

        public override void Initialize()
        {
            _timerService.TimerTick += OnTimerTick;
            _timerService.TimerCompleted += OnTimerCompleted;
            
            // Load settings and update display
            _settingsService.LoadSettings();
            TimeDisplay = $"{_settingsService.FocusDuration}:00";
            Progress = 0;
            
            base.Initialize();
        }

        [RelayCommand(CanExecute = nameof(CanStart))]
        private void Start()
        {
            if (CurrentSession == null)
            {
                CurrentSession = new PomodoroSession(_settingsService.FocusDuration, SessionType.Focus);
            }
            _timerService.Start();
            IsRunning = true;
        }

        private bool CanStart() => !IsRunning;

        [RelayCommand(CanExecute = nameof(CanPause))]
        private void Pause()
        {
            _timerService.Pause();
            IsRunning = false;
        }

        private bool CanPause() => IsRunning;

        [RelayCommand]
        private void Reset()
        {
            _timerService.Reset();
            IsRunning = false;
            CurrentSession = null;
            TimeDisplay = $"{_settingsService.FocusDuration}:00";
            Progress = 0;
        }

        private void OnTimerTick(object? sender, TimeSpan remaining)
        {
            TimeDisplay = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            if (CurrentSession != null)
            {
                var elapsed = TimeSpan.FromMinutes(CurrentSession.Duration) - remaining;
                Progress = elapsed.TotalSeconds / (CurrentSession.Duration * 60);
            }
        }

        private void OnTimerCompleted(object? sender, EventArgs e)
        {
            if (CurrentSession != null)
            {
                CurrentSession.IsCompleted = true;
                CurrentSession.EndTime = DateTime.Now;
            }
            _notificationService.ShowNotification("Time's up!", "Take a break!");
            Progress = 1.0;
            IsRunning = false;
        }
    }
}
