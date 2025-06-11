# TaskManagement.Mobile UI Tests

This guide explains how to set up and run the UI tests for the TaskManagement.Mobile app using Appium and NUnit.

## Prerequisites

1. **.NET SDK 9.0+**  
   [Download and install .NET 9.0 SDK](https://dotnet.microsoft.com/download)

2. **Android Studio & Emulator**
   - [Download Android Studio](https://developer.android.com/studio)
   - Use the AVD Manager to create and start an Android Emulator (API 30+ recommended).
   - Ensure both the emulator and the API backend are running.

3. **Appium Server & Android Driver**
   - [Install Node.js](https://nodejs.org/)
   - Install Appium globally:
     ```
     npm install -g appium
     ```
   - Install the Android UIAutomator2 driver:
     ```
     appium driver install uiautomator2
     ```
   - (Optional) Install Appium Doctor to verify your environment:
     ```
     npm install -g appium-doctor
     appium-doctor
     ```
   - (Optional) Install Android platform tools if not already present:
     ```
     npm install -g adbkit
     ```
   - Start the Appium server:
     ```
     appium
     ```
     * Make sure appium is properly set in your PATH.
  - See the [Appium Documentation](https://appium.io/docs/en/latest/quickstart/) for more details.

4. **Build and Deploy the App**
   - Build your mobile app and deploy it to the emulator.
   - Ensure the app package name matches `com.companyname.taskmanagement.mobileapp`.

## Running the Tests

1. **Ensure the Emulator and API are Running**
   - Start the Android emulator and confirm the app is installed and can be launched.
   - Make sure the backend API is running and accessible, and all tables are seeded.
   - You should also be authenticated. So the user should be on the main task overview.

2. **Set Environment Variables (Optional)**
   - If Appium is not running on the default `http://127.0.0.1:4723/`, set the `APPIUM_HOST` environment variable.

3. **Run the Tests**
   - Navigate to the test project directory:
     ```
     cd src/Mobile/TaskManagement.Mobile.Tests
     ```
   - Run the tests using the .NET CLI:
     ```
     dotnet test
     ```

## Troubleshooting

### Appium/Emulator Cannot Find Android SDK

If you encounter this error:  `An unknown server-side error occurred while processing the command. Original error: Neither ANDROID_HOME nor ANDROID_SDK_ROOT environment variable was exported".`

Ensure that the `ANDROID_HOME` or `ANDROID_SDK_ROOT` environment variable is set and points to your Android SDK location.

**How to set on Windows (Command Prompt):**

```
:: Set ANDROID_HOME (replace <YourUser> with your username)
setx ANDROID_HOME "C:\Users\<YourUser>\AppData\Local\Android\Sdk"

:: Add platform-tools to your PATH (run in a new terminal after setting ANDROID_HOME)
setx PATH "%PATH%;%ANDROID_HOME%\platform-tools"
```
* And make sure to restart the IDE before trying again!