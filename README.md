# AutoClicker (ASClicker)

![Build Status](https://github.com/mrdevil786/ASClicker/actions/workflows/build.yml/badge.svg)

A simple Windows application that simulates mouse clicks at specified intervals.

## Features

- Start/Stop clicking with global hotkey (Ctrl+Shift+A)
- Adjustable click interval in milliseconds
- System tray icon for minimized operation
- Simulates left mouse clicks at the current cursor position
- Background operation with minimal resource usage
- Single instance application (prevents multiple instances)

## How to Use

1. Download the latest release from the [Releases](https://github.com/mrdevil786/ASClicker/releases) page
2. Launch the application (AutoClicker.exe)
3. Set the click interval in milliseconds (default: 100ms)
4. Press the "Start" button or use the global hotkey (Ctrl+Shift+A) to start clicking
5. Press the same hotkey (Ctrl+Shift+A) to stop clicking
6. The application can be minimized to the system tray
7. Right-click the tray icon to show the application or exit it

## Build Requirements

- .NET Framework 4.7.2 or higher
- Windows operating system
- Visual Studio (for development)

## Building from Source

1. Clone the repository:
   ```
   git clone https://github.com/mrdevil786/ASClicker.git
   ```
2. Open the solution in Visual Studio
3. Build the solution (Ctrl+Shift+B)
4. The compiled executable will be in the bin/Release directory

## Automated Build

This project uses GitHub Actions for continuous integration. Each push to the repository triggers an automated build process. You can download the latest build artifacts from the Actions tab on GitHub.

## Notes

- When you close the application window, it will minimize to the system tray rather than exit
- To fully exit the application, right-click the tray icon and select "Exit"
- The hotkey works even when the application is minimized or not in focus

## License

MIT License 