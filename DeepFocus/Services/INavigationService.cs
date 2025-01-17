using System;
using Microsoft.UI.Xaml.Controls;

namespace DeepFocus.Services
{
    public interface INavigationService
    {
        Frame? Frame { get; set; }
        bool CanGoBack { get; }
        event EventHandler<string>? Navigated;
        bool GoBack();
        void RegisterPage(string key, Type pageType);
        bool NavigateTo(string pageKey, object? parameter = null);
    }
}
