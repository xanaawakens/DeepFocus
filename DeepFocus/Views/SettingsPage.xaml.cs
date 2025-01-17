using System;
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
        }
    }
}
