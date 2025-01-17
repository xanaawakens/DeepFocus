using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace DeepFocus.Services
{
    public class SoundManager
    {
        private readonly Dictionary<string, MediaPlayer> _soundPlayers = new();
        private readonly string _soundsPath;
        private double _volume = 1.0;

        public static readonly IReadOnlyList<string> AvailableSounds = new List<string>
        {
            "Bell",
            "Chime",
            "Ding",
            "Gong",
            "Meditation",
            "Notification",
            "Success",
            "Zen"
        };

        public double Volume
        {
            get => _volume;
            set
            {
                _volume = Math.Clamp(value, 0.0, 1.0);
                UpdatePlayersVolume();
            }
        }

        public SoundManager()
        {
            _soundsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Sounds");
            EnsureSoundsExist().Wait();
            InitializeSoundPlayers();
        }

        private async Task EnsureSoundsExist()
        {
            try
            {
                // Create sounds directory if it doesn't exist
                if (!Directory.Exists(_soundsPath))
                {
                    Directory.CreateDirectory(_soundsPath);
                }

                // Copy sound files from installation directory if they don't exist
                var installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var soundsFolder = await installFolder.GetFolderAsync("Assets\\Sounds");

                foreach (var soundName in AvailableSounds)
                {
                    var fileName = $"{soundName.ToLowerInvariant()}.wav";
                    var targetPath = Path.Combine(_soundsPath, fileName);

                    if (!File.Exists(targetPath))
                    {
                        var sourceFile = await soundsFolder.GetFileAsync(fileName);
                        await sourceFile.CopyAsync(await StorageFolder.GetFolderFromPathAsync(_soundsPath));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ensuring sounds exist: {ex.Message}");
            }
        }

        private void InitializeSoundPlayers()
        {
            foreach (var sound in AvailableSounds)
            {
                try
                {
                    var fileName = $"{sound.ToLowerInvariant()}.wav";
                    var filePath = Path.Combine(_soundsPath, fileName);
                    
                    if (File.Exists(filePath))
                    {
                        var player = new MediaPlayer
                        {
                            Source = MediaSource.CreateFromUri(new Uri(filePath)),
                            AudioCategory = MediaPlayerAudioCategory.Alerts,
                            Volume = _volume
                        };
                        _soundPlayers[sound] = player;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error initializing sound player for {sound}: {ex.Message}");
                }
            }
        }

        private void UpdatePlayersVolume()
        {
            foreach (var player in _soundPlayers.Values)
            {
                player.Volume = _volume;
            }
        }

        public void PlaySound(string soundName, double? volumeOverride = null)
        {
            if (_soundPlayers.TryGetValue(soundName, out var player))
            {
                try
                {
                    if (volumeOverride.HasValue)
                    {
                        player.Volume = Math.Clamp(volumeOverride.Value, 0.0, 1.0);
                    }

                    player.PlaybackSession.Position = TimeSpan.Zero;
                    player.Play();

                    if (volumeOverride.HasValue)
                    {
                        player.Volume = _volume; // Reset to default volume after playing
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error playing sound {soundName}: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            foreach (var player in _soundPlayers.Values)
            {
                player.Dispose();
            }
            _soundPlayers.Clear();
        }
    }
}
