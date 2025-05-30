# AutoClicker (ASAutoClicker)

![Build Status](https://github.com/mrdevil786/ASAutoClicker/actions/workflows/build.yml/badge.svg)

A simple Windows application that simulates mouse clicks at specified intervals.

## Features

- Start/Stop clicking with global hotkey (Ctrl+Shift+A)
- Adjustable click interval in milliseconds
- Selectable mouse button (Left, Right, Middle)
- Click limit options:
  - Click indefinitely
  - Stop after a specific number of clicks
  - Stop after a specific duration (in seconds)
- System tray icon for minimized operation
- Simulates mouse clicks at the current cursor position
- Background operation with minimal resource usage
- Single instance application (prevents multiple instances)
- Proper thread-safe implementation for reliable operation
- Improved resource management and cleanup

## How to Use

1. Download the latest release from the [Releases](https://github.com/mrdevil786/ASAutoClicker/releases) page
2. Launch the application (AutoClicker.exe)
3. Set the click interval in milliseconds (default: 100ms)
4. Choose the desired click options:
   - **Click Indefinitely:** Clicks will continue until stopped manually.
   - **Number of Clicks:** Set the total number of clicks to perform.
   - **Duration (secs):** Set the duration in seconds for which clicking should occur.
5. Select the mouse button (Left, Right, or Middle) to simulate.
6. Press the "Start" button or use the global hotkey (Ctrl+Shift+A) to start clicking.
7. Press the same hotkey (Ctrl+Shift+A) to stop clicking manually, or wait for the set limit (if applicable).
8. The application can be minimized to the system tray.
9. Right-click the tray icon to show the application or exit it.

## Recent Improvements & Optimizations

The codebase has been optimized with the following improvements:

1. **Enhanced Click Mechanism**: Improved the mouse click simulation with separate down/up events
2. **Thread Safety**: Added proper thread synchronization to prevent race conditions
3. **Better Error Handling**: Added robust error handling throughout the application
4. **Resource Management**: Improved resource disposal and cleanup
5. **Performance Optimization**: More efficient background worker implementation
6. **UI Responsiveness**: Proper use of UI thread invocation for smoother experience
7. **Hotkey Registration**: Added better error handling for hotkey registration
8. **Single Instance Management**: Improved the single instance activation logic
9. **Click Duration Tracking**: Fixed issues with stopwatch for duration tracking
10. **Memory Usage**: Optimized memory usage patterns for long-running operation

## Windows Security Warning

When you first run AutoClicker, Windows Defender SmartScreen may display a warning. This is normal for smaller applications and utilities.

**To run the application when this warning appears:**
1. Click on "More info" in the warning dialog
2. Click "Run anyway"

The warning typically only appears the first time you run the application.

## Antivirus False Positives

Some antivirus programs, including Windows Defender, may flag AutoClicker as a potential threat (such as "Trojan:Win32/Wacatac.C!ml"). This is a **false positive** because:

1. AutoClicker simulates mouse clicks programmatically, which is behavior similar to some malware
2. It uses low-level system hooks to capture and simulate input events
3. Many auto-clicker tools face the same issue with antivirus software

**If your antivirus flags this application:**

1. **Add an exclusion** in your antivirus settings for the AutoClicker.exe file
2. **Report the false positive** to your antivirus vendor
3. If you're concerned, you can build the application from source to ensure it's safe

### Code Improvements to Reduce Detections

To minimize false positive detections, we've implemented several best practices:

1. Using modern Windows APIs (`SendInput` instead of `mouse_event`)
2. Adding human-like randomization to click timing (can be enabled in code)
3. Implementing proper error handling and code structure
4. Providing detailed assembly information and documentation
5. Proper resource cleanup and exception handling
6. Thread-safe implementation with synchronization mechanisms

This application is open-source, so you can review all the code to verify its safety.

## Build Requirements

- .NET Framework 4.7.2 or higher
- Windows operating system
- Visual Studio (for development)

## Building from Source

1. Clone the repository:
   ```
   git clone https://github.com/mrdevil786/ASAutoClicker.git
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
- The application is thread-safe and efficiently manages system resources

## License

MIT License