using System;
using System.Timers;
using Microsoft.UI.Dispatching;

namespace DeepFocus.Services
{
    public class TimerService : ITimerService, IDisposable
    {
        private readonly Timer _timer;
        private readonly DispatcherQueue _dispatcherQueue;
        private TimeSpan _remaining;
        private DateTime _pauseTime;
        private bool _isPaused;

        public event EventHandler<TimeSpan>? TimerTick;
        public event EventHandler? TimerCompleted;

        public TimerService()
        {
            _timer = new Timer(1000); // Tick every second
            _timer.Elapsed += Timer_Elapsed;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _remaining = TimeSpan.FromMinutes(25); // Default duration
        }

        public void Start()
        {
            if (_isPaused)
            {
                // Adjust remaining time based on pause duration
                var pauseDuration = DateTime.Now - _pauseTime;
                _remaining = _remaining;
                _isPaused = false;
            }
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
            _pauseTime = DateTime.Now;
            _isPaused = true;
        }

        public void Reset()
        {
            _timer.Stop();
            _remaining = TimeSpan.FromMinutes(25);
            _isPaused = false;
            OnTimerTick();
        }

        public void SetDuration(int minutes)
        {
            _remaining = TimeSpan.FromMinutes(minutes);
            OnTimerTick();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            _remaining = _remaining.Subtract(TimeSpan.FromSeconds(1));
            
            if (_remaining <= TimeSpan.Zero)
            {
                _timer.Stop();
                _remaining = TimeSpan.Zero;
                OnTimerCompleted();
            }
            
            OnTimerTick();
        }

        private void OnTimerTick()
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                TimerTick?.Invoke(this, _remaining);
            });
        }

        private void OnTimerCompleted()
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                TimerCompleted?.Invoke(this, EventArgs.Empty);
            });
        }

        public void Dispose()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
        }
    }
}
