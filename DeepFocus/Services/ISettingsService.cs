namespace DeepFocus.Services
{
    public interface ISettingsService
    {
        // Timer Settings
        int FocusDuration { get; set; }
        int ShortBreakDuration { get; set; }
        int LongBreakDuration { get; set; }
        int CyclesBeforeLongBreak { get; set; }

        // Theme Settings
        bool IsDarkMode { get; set; }

        // Notification Settings
        bool PlayNotificationSounds { get; set; }
        string NotificationSound { get; set; }
        bool ShowToastNotifications { get; set; }

        // Automation Settings
        bool AutoStartBreaks { get; set; }
        bool AutoStartNextPomodoro { get; set; }
        
        void SaveSettings();
        void LoadSettings();
    }
}
