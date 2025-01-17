using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using DeepFocus.Views;

namespace DeepFocus.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _frame;
        private readonly Dictionary<string, Type> _pages = new();
        public event EventHandler<string>? Navigated;

        public Frame? Frame
        {
            get => _frame;
            set
            {
                if (_frame == value)
                    return;

                if (_frame != null)
                    _frame.Navigated -= OnNavigated;

                _frame = value;

                if (_frame != null)
                    _frame.Navigated += OnNavigated;
            }
        }

        public bool CanGoBack => Frame != null && Frame.CanGoBack;

        public NavigationService()
        {
            RegisterPages();
        }

        private void RegisterPages()
        {
            RegisterPage("timer", typeof(MainPage));
            RegisterPage("statistics", typeof(StatisticsPage));
            RegisterPage("settings", typeof(SettingsPage));
        }

        public void RegisterPage(string key, Type pageType)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Page key cannot be null or whitespace", nameof(key));

            if (pageType == null)
                throw new ArgumentNullException(nameof(pageType));

            _pages[key] = pageType;
        }

        public bool GoBack()
        {
            if (Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
                return true;
            }
            return false;
        }

        public bool NavigateTo(string pageKey, object? parameter = null)
        {
            if (_frame == null)
                return false;

            if (!_pages.TryGetValue(pageKey, out var pageType))
                return false;

            return _frame.Navigate(pageType, parameter);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == null)
                return;

            var pageKey = _pages.FirstOrDefault(x => x.Value == e.Content.GetType()).Key;
            if (!string.IsNullOrEmpty(pageKey))
                Navigated?.Invoke(this, pageKey);
        }
    }
}
