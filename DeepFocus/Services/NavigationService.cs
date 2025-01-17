using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using DeepFocus.Views;

namespace DeepFocus.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _pages = new();
        private Frame _frame;

        public event NavigatedEventHandler Navigated;

        public Frame Frame
        {
            get => _frame;
            set
            {
                _frame = value;
                _frame.Navigated += Frame_Navigated;
            }
        }

        public bool CanGoBack => Frame != null && Frame.CanGoBack;

        public NavigationService()
        {
            RegisterPages();
        }

        private void RegisterPages()
        {
            _pages.Add("timer", typeof(MainPage));
            _pages.Add("statistics", typeof(StatisticsPage));
            _pages.Add("settings", typeof(SettingsPage));
        }

        public bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }
            return false;
        }

        public bool NavigateTo(string pageKey, object parameter = null)
        {
            if (_pages.ContainsKey(pageKey))
            {
                return Frame.Navigate(_pages[pageKey], parameter);
            }
            return false;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(sender, e);
        }
    }
}
