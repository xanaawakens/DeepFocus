using Microsoft.UI.Xaml.Controls;
using DeepFocus.ViewModels;

namespace DeepFocus.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService(typeof(SettingsViewModel)) as SettingsViewModel
                ?? throw new InvalidOperationException("SettingsViewModel not found in DI container");
            
            this.Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
