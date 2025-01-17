using CommunityToolkit.Mvvm.ComponentModel;

namespace DeepFocus.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected ViewModelBase()
        {
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isInitialized;
        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public virtual void Initialize()
        {
            IsInitialized = true;
        }
    }
}
