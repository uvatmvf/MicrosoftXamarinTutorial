using DroneLander.Services;
using DroneLander.UWP.DependencyServices;
using System;
using Windows.Media.Playback;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace DroneLander.UWP.DependencyServices
{
    public class AudioService : IAudioService
    {
        private MediaPlayer _mediaPlayer;

        public Action OnFinishedPlaying { get; set; }

        public AudioService()
        {
        }

        public void AdjustVolume(double level)
        {
            float volume = (float)(level / 100.0);

            _mediaPlayer.Volume = volume;
        }
        public void KillEngine()
        {
            _mediaPlayer.Volume = 0.0;
        }

        public void ToggleEngine()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Pause();

                var session = _mediaPlayer.PlaybackSession;
                session.Position = new TimeSpan(0);
                _mediaPlayer.MediaEnded -= OnMediaEnded;

            }
            else
            {
                var fullPath = "Assets/Sounds/engine.m4a";

                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.AutoPlay = false;
                _mediaPlayer.Volume = 0.1f;
                _mediaPlayer.MediaEnded += OnMediaEnded;

                Uri pathUri = new Uri($"ms-appx:///{fullPath}");
                _mediaPlayer.Source = Windows.Media.Core.MediaSource.CreateFromUri(pathUri);
                _mediaPlayer.Play();
            }
        }

        private void OnMediaEnded(MediaPlayer sender, object args)
        {
            OnFinishedPlaying?.Invoke();
        }
    }
}