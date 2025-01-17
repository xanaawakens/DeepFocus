# DeepFocus - Modern Pomodoro Timer

DeepFocus is a modern, feature-rich Pomodoro timer application built with WinUI 3 and .NET. It helps users maintain focus and productivity through the time-tested Pomodoro Technique, wrapped in a beautiful and intuitive interface.

## Features

- 🎯 **Intuitive Timer Interface**
  - Clean, modern design with clear timer display
  - Easy-to-use controls (Start, Pause, Reset)
  - Visual cycle tracking (e.g., "Pomodoro 3/4")
  - Dark mode support

- ⚙️ **Customization**
  - Adjustable Pomodoro duration
  - Configurable short and long break periods
  - Customizable number of cycles before long breaks

- 🔔 **Smart Notifications**
  - Toast notifications when timer completes
  - Customizable alert sounds
  - Non-intrusive reminders

- 📊 **Statistics & Insights**
  - Visual charts showing daily/weekly progress
  - Track completed Pomodoros
  - Performance analytics

- 💾 **Data Management**
  - Local data storage using SQLite
  - Automatic saving of preferences
  - Session history tracking

## System Requirements

- Windows 10 version 1809 or higher
- .NET 8.0 or later
- Windows App SDK 1.4 or later

## Installation

1. Download the latest release from the releases page
2. Run the installer package (`.msix`)
3. Follow the installation prompts
4. Launch DeepFocus from the Start menu

## Development Setup

1. Install Visual Studio 2022 (17.8 or later)
2. Install the following workloads:
   - .NET Desktop Development
   - Universal Windows Platform development
   - Windows App SDK C# templates

3. Clone the repository:
```bash
git clone git@github.com:avixiii-dev/DeepFocus.git
```

4. Open `DeepFocus.sln` in Visual Studio
5. Build and run the application

## Project Structure

- `DeepFocus/`
  - `Models/` - Data models and business logic
  - `ViewModels/` - MVVM view models
  - `Views/` - XAML views and pages
  - `Services/` - Core services (notifications, storage, etc.)
  - `Helpers/` - Utility classes and extensions

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [Windows App SDK](https://github.com/microsoft/WindowsAppSDK)
- Uses [CommunityToolkit.WinUI](https://github.com/CommunityToolkit/WindowsCommunityToolkit)
- Inspired by the [Pomodoro Technique®](https://francescocirillo.com/pages/pomodoro-technique)
