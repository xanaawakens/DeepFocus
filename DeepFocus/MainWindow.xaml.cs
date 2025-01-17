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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeepFocus
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();

            // Get AppWindow
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Customize titlebar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            // Set initial size
            appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1000, Height = 600 });

            // Set minimum size
            var presenter = appWindow.Presenter as OverlappedPresenter;
            presenter.IsResizable = true;
            presenter.IsMaximizable = true;
            presenter.IsMinimizable = true;
            presenter.SetMinSize(new Windows.Graphics.SizeInt32 { Width = 500, Height = 400 });

            // Initialize ViewModel
            ViewModel = App.Current.Services.GetService(typeof(MainViewModel)) as MainViewModel 
                ?? throw new InvalidOperationException("MainViewModel not found in DI container");
        }
    }
}
