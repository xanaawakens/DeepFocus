using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Windows.Foundation;
using Windows.Foundation.Collections;
using DeepFocus.ViewModels;
using DeepFocus.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeepFocus
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private const int DefaultWidth = 800;
        private const int DefaultHeight = 600;
        private const int MinWidth = 400;
        private const int MinHeight = 300;

        private readonly INavigationService _navigationService;
        private readonly AppWindow? _appWindow;
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();

            // Get AppWindow
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Customize titlebar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            // Set window size and constraints
            if (_appWindow != null)
            {
                var size = new Windows.Graphics.SizeInt32(DefaultWidth, DefaultHeight);
                _appWindow.Resize(size);

                var presenter = _appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsResizable = true;
                    presenter.IsMaximizable = true;
                    presenter.IsMinimizable = true;
                }
            }

            // Get navigation service and set up frame
            _navigationService = App.Current.Services.GetService(typeof(INavigationService)) as INavigationService
                ?? throw new InvalidOperationException("NavigationService not found in DI container");
            _navigationService.Frame = ContentFrame;

            // Initialize ViewModel
            ViewModel = App.Current.Services.GetService(typeof(MainViewModel)) as MainViewModel 
                ?? throw new InvalidOperationException("MainViewModel not found in DI container");

            // Navigate to default page
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
            _navigationService.NavigateTo("timer");
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem selectedItem)
            {
                string pageKey = selectedItem.Tag?.ToString() ?? string.Empty;
                _navigationService.NavigateTo(pageKey);
            }
        }
    }
}
