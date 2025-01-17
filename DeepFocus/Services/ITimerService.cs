using System;

namespace DeepFocus.Services
{
    public interface ITimerService
    {
        event EventHandler<TimeSpan> TimerTick;
        event EventHandler TimerCompleted;
        
        void Start();
        void Pause();
        void Reset();
        void SetDuration(int minutes);
    }
}
