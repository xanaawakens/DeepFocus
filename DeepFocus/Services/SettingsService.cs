using System;
using System.IO;
using System.Text.Json;
using Windows.Storage;

namespace DeepFocus.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsFileName = "settings.json";
        private Settings _settings;

        private class Settings
        {
            public int FocusDuration { get; set; } = 25;
            public int ShortBreakDuration { get; set; } = 5;
            public int LongBreakDuration { get; set; } = 15;
            public int CyclesBeforeLongBreak { get; set; } = 4;
            public bool IsDarkMode { get; set; } = false;
            public bool PlayNotificationSounds { get; set; } = true;
            public string NotificationSound { get; set; } = "Default";
            public bool ShowToastNotifications { get; set; } = true;
            public bool AutoStartBreaks { get; set; } = false;
            public bool AutoStartNextPomodoro { get; set; } = false;
        }

        public SettingsService()
        {
            _settings = new Settings();
            LoadSettings();
        }

        public int FocusDuration
        {
            get => _settings.FocusDuration;
            set => _settings.FocusDuration = value;
        }

        public int ShortBreakDuration
        {
            get => _settings.ShortBreakDuration;
            set => _settings.ShortBreakDuration = value;
        }

        public int LongBreakDuration
        {
            get => _settings.LongBreakDuration;
            set => _settings.LongBreakDuration = value;
        }

        public int CyclesBeforeLongBreak
        {
            get => _settings.CyclesBeforeLongBreak;
            set => _settings.CyclesBeforeLongBreak = value;
        }

        public bool IsDarkMode
        {
            get => _settings.IsDarkMode;
            set => _settings.IsDarkMode = value;
        }

        public bool PlayNotificationSounds
        {
            get => _settings.PlayNotificationSounds;
            set => _settings.PlayNotificationSounds = value;
        }

        public string NotificationSound
        {
            get => _settings.NotificationSound;
            set => _settings.NotificationSound = value;
        }

        public bool ShowToastNotifications
        {
            get => _settings.ShowToastNotifications;
            set => _settings.ShowToastNotifications = value;
        }

        public bool AutoStartBreaks
        {
            get => _settings.AutoStartBreaks;
            set => _settings.AutoStartBreaks = value;
        }

        public bool AutoStartNextPomodoro
        {
            get => _settings.AutoStartNextPomodoro;
            set => _settings.AutoStartNextPomodoro = value;
        }

        public void SaveSettings()
        {
            try
            {
                var settingsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, SettingsFileName);
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public void LoadSettings()
        {
            try
            {
                var settingsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, SettingsFileName);
                if (File.Exists(settingsPath))
                {
                    var json = File.ReadAllText(settingsPath);
                    _settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception ex)
            {
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
                _settings = new Settings();
            }
        }
    }
}
