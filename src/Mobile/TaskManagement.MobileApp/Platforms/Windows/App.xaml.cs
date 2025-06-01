using Microsoft.UI.Xaml;
using TaskManagement.MobileApp.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TaskManagement.MobileApp.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    const int WindowWidth = 400;
    const int WindowHeight = 750;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            //  * https://stackoverflow.com/questions/72399551/maui-net-set-window-size
            //  * Found this snippet on stackoverflow; it sets the default size.
            //  * This App is meant for mobile devices So we atleast try to mimic that here
            //  * you can still resize it but... it gets the point across
            #if WINDOWS
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));
            #endif
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}