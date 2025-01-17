using System;
using Microsoft.UI.Xaml.Controls;
using DeepFocus.ViewModels;

namespace DeepFocus.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService(typeof(MainViewModel)) as MainViewModel
                ?? throw new InvalidOperationException("MainViewModel not found in DI container");
            
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
