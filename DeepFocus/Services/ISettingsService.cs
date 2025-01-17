namespace DeepFocus.Services
{
    public interface ISettingsService
    {
        int FocusDuration { get; set; }
        int ShortBreakDuration { get; set; }
        int LongBreakDuration { get; set; }
        int CyclesBeforeLongBreak { get; set; }
        bool IsDarkMode { get; set; }
        
        void SaveSettings();
        void LoadSettings();
    }
}
