using System;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace DeepFocus.Services
{
    public class NotificationService : INotificationService, IDisposable
    {
        private readonly SoundManager _soundManager;
        private readonly AppNotificationManager _notificationManager;

        public NotificationService()
        {
            _soundManager = new SoundManager();
            _notificationManager = AppNotificationManager.Default;
            _notificationManager.Register();
        }

        public void ShowNotification(string title, string message)
        {
            try
            {
                var builder = new AppNotificationBuilder()
                    .AddText(title)
                    .AddText(message);

                var notification = builder.BuildNotification();
                _notificationManager.Show(notification);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing notification: {ex.Message}");
            }
        }

        public void PlaySound(string soundName)
        {
            _soundManager.PlaySound(soundName);
        }

        public string[] GetAvailableSounds()
        {
            return SoundManager.AvailableSounds.ToArray();
        }

        public void Dispose()
        {
            _soundManager.Dispose();
            _notificationManager.Unregister();
        }
    }
}
