using System;
using Microsoft.UI.Xaml.Controls;
using DeepFocus.ViewModels;

namespace DeepFocus.Views
{
    public sealed partial class StatisticsPage : Page
    {
        public StatisticsViewModel ViewModel { get; }

        public StatisticsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService(typeof(StatisticsViewModel)) as StatisticsViewModel
                ?? throw new InvalidOperationException("StatisticsViewModel not found in DI container");
            
            this.Loaded += StatisticsPage_Loaded;
        }

        private async void StatisticsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadStatisticsAsync();
        }
    }
}
