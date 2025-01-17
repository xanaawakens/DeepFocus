namespace DeepFocus.Services
{
    public interface INotificationService
    {
        void ShowNotification(string title, string message);
        void PlaySound(string soundName);
        string[] GetAvailableSounds();
    }
}
