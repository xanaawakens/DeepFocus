using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepFocus.Services;
using Microsoft.UI.Xaml;

namespace DeepFocus.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private int focusDuration;

        [ObservableProperty]
        private int shortBreakDuration;

        [ObservableProperty]
        private int longBreakDuration;

        [ObservableProperty]
        private int cyclesBeforeLongBreak;

        [ObservableProperty]
        private bool isDarkMode;

        [ObservableProperty]
        private bool playNotificationSounds;

        [ObservableProperty]
        private string selectedSound = "Default";

        [ObservableProperty]
        private double notificationVolume = 1.0;

        [ObservableProperty]
        private bool showToastNotifications = true;

        [ObservableProperty]
        private bool autoStartBreaks;

        [ObservableProperty]
        private bool autoStartNextPomodoro;

        public ObservableCollection<string> AvailableSounds { get; }

        public SettingsViewModel(ISettingsService settingsService, INotificationService notificationService)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
            Title = "Settings";

            // Initialize available sounds
            AvailableSounds = new ObservableCollection<string>(_notificationService.GetAvailableSounds());
        }

        public override void Initialize()
        {
            LoadSettings();
            base.Initialize();
        }

        private void LoadSettings()
        {
            _settingsService.LoadSettings();
            
            FocusDuration = _settingsService.FocusDuration;
            ShortBreakDuration = _settingsService.ShortBreakDuration;
            LongBreakDuration = _settingsService.LongBreakDuration;
            CyclesBeforeLongBreak = _settingsService.CyclesBeforeLongBreak;
            IsDarkMode = _settingsService.IsDarkMode;

            // Load additional settings
            PlayNotificationSounds = _settingsService.PlayNotificationSounds;
            SelectedSound = _settingsService.NotificationSound;
            NotificationVolume = _notificationService.Volume;
            ShowToastNotifications = _settingsService.ShowToastNotifications;
            AutoStartBreaks = _settingsService.AutoStartBreaks;
            AutoStartNextPomodoro = _settingsService.AutoStartNextPomodoro;

            // Ensure selected sound is valid
            if (!AvailableSounds.Contains(SelectedSound))
            {
                SelectedSound = AvailableSounds[0];
            }
        }

        [RelayCommand]
        private void SaveSettings()
        {
            _settingsService.FocusDuration = FocusDuration;
            _settingsService.ShortBreakDuration = ShortBreakDuration;
            _settingsService.LongBreakDuration = LongBreakDuration;
            _settingsService.CyclesBeforeLongBreak = CyclesBeforeLongBreak;
            _settingsService.IsDarkMode = IsDarkMode;

            // Save additional settings
            _settingsService.PlayNotificationSounds = PlayNotificationSounds;
            _settingsService.NotificationSound = SelectedSound;
            _notificationService.Volume = NotificationVolume;
            _settingsService.ShowToastNotifications = ShowToastNotifications;
            _settingsService.AutoStartBreaks = AutoStartBreaks;
            _settingsService.AutoStartNextPomodoro = AutoStartNextPomodoro;

            _settingsService.SaveSettings();
            _notificationService.ShowNotification("Settings Saved", "Your preferences have been updated.");
        }

        [RelayCommand]
        private void TestNotification()
        {
            if (PlayNotificationSounds)
            {
                _notificationService.PlaySound(SelectedSound, NotificationVolume);
            }
            if (ShowToastNotifications)
            {
                _notificationService.ShowNotification("Test Notification", "This is a test notification.");
            }
        }

        [RelayCommand]
        private void ResetToDefaults()
        {
            FocusDuration = 25;
            ShortBreakDuration = 5;
            LongBreakDuration = 15;
            CyclesBeforeLongBreak = 4;
            PlayNotificationSounds = true;
            SelectedSound = AvailableSounds[0];
            NotificationVolume = 1.0;
            ShowToastNotifications = true;
            AutoStartBreaks = false;
            AutoStartNextPomodoro = false;
        }
    }
}
