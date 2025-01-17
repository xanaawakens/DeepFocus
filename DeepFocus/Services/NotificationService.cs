using System;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.Storage;

namespace DeepFocus.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly AppNotificationManager _notificationManager;

        public NotificationService()
        {
            _mediaPlayer = new MediaPlayer();
            _notificationManager = AppNotificationManager.Default;
            
            // Register the app for notifications
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
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error showing notification: {ex.Message}");
            }
        }

        public async void PlaySound(string soundName)
        {
            try
            {
                // Get the sound file from the app's assets
                var folder = await StorageFolder.GetFolderFromPathAsync("Assets");
                var file = await folder.GetFileAsync($"{soundName}.mp3");
                
                // Create a MediaSource from the file and play it
                _mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
                _mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                // In a production app, we would want to log this error
                System.Diagnostics.Debug.WriteLine($"Error playing sound: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _mediaPlayer.Dispose();
            _notificationManager.Unregister();
        }
    }
}
