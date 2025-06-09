using Android.App;
using Android.Runtime;

namespace TaskManagement.MobileApp;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        EntryUnderlineRemover.Init();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}