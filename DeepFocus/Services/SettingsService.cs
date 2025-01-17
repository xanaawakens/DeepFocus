using System;
using System.IO;
using System.Text.Json;
using Windows.Storage;

namespace DeepFocus.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsFileName = "settings.json";
        private readonly string _settingsPath;
        
        public int FocusDuration { get; set; } = 25;
        public int ShortBreakDuration { get; set; } = 5;
        public int LongBreakDuration { get; set; } = 15;
        public int CyclesBeforeLongBreak { get; set; } = 4;
        public bool IsDarkMode { get; set; } = false;

        public SettingsService()
        {
            _settingsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, SettingsFileName);
            LoadSettings();
        }

        public void SaveSettings()
        {
            try
            {
                var settings = new
                {
                    FocusDuration,
                    ShortBreakDuration,
                    LongBreakDuration,
                    CyclesBeforeLongBreak,
                    IsDarkMode
                };

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                File.WriteAllText(_settingsPath, json);
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
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    var settings = JsonSerializer.Deserialize<SettingsModel>(json);
                    
                    if (settings != null)
                    {
                        FocusDuration = settings.FocusDuration;
                        ShortBreakDuration = settings.ShortBreakDuration;
                        LongBreakDuration = settings.LongBreakDuration;
                        CyclesBeforeLongBreak = settings.CyclesBeforeLongBreak;
                        IsDarkMode = settings.IsDarkMode;
                    }
                }
            }
            catch (Exception ex)
            {
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
                // Use default values if loading fails
            }
        }

        private class SettingsModel
        {
            public int FocusDuration { get; set; }
            public int ShortBreakDuration { get; set; }
            public int LongBreakDuration { get; set; }
            public int CyclesBeforeLongBreak { get; set; }
            public bool IsDarkMode { get; set; }
        }
    }
}
