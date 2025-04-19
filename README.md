# AutoClicker

A simple Windows application that simulates mouse clicks at specified intervals.

## Features

- Start/Stop clicking with global hotkey (Ctrl+Shift+A)
- Adjustable click interval in milliseconds
- System tray icon for minimized operation
- Simulates left mouse clicks at the current cursor position

## How to Use

1. Launch the application
2. Set the click interval in milliseconds (default: 100ms)
3. Press the "Start" button or use the global hotkey (Ctrl+Shift+A) to start clicking
4. Press the same hotkey (Ctrl+Shift+A) to stop clicking
5. The application can be minimized to the system tray
6. Right-click the tray icon to exit the application

## Build Requirements

- .NET Framework 4.7.2 or higher
- Windows operating system

## Notes

- When you close the application window, it will minimize to the system tray rather than exit
- To fully exit the application, right-click the tray icon and select "Exit"
- The hotkey works even when the application is minimized or not in focus 