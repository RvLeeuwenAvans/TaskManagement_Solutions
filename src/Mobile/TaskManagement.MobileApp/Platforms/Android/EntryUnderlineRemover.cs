using Android.Graphics.Drawables;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace TaskManagement.MobileApp;

public static class EntryUnderlineRemover
{
    public static void Init()
    {
        EntryHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, view) =>
        {
            if (handler.PlatformView is not Android.Widget.EditText editText)
                return;

            // Remove underline
            editText.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            // Apply background color again if set
            if (view is Entry { BackgroundColor: not null } mauiEntry)
            {
                editText.SetBackgroundColor(mauiEntry.BackgroundColor.ToPlatform());
            }
        });
        
        PickerHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, view) =>
        {
            if (handler.PlatformView is not Android.Widget.EditText editText)
                return;
            
            editText.Background = new ColorDrawable(Android.Graphics.Color.Transparent);

            if (view is Picker { BackgroundColor: not null } mauiEntry)
            {
                editText.SetBackgroundColor(mauiEntry.BackgroundColor.ToPlatform());
            }
        });
        
        DatePickerHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, view) =>
        {
            if (handler.PlatformView is not Android.Widget.EditText editText)
                return;
            
            editText.Background = new ColorDrawable(Android.Graphics.Color.Transparent);

            if (view is DatePicker { BackgroundColor: not null } mauiEntry)
            {
                editText.SetBackgroundColor(mauiEntry.BackgroundColor.ToPlatform());
            }
        });
        
        EditorHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, view) =>
        {
            if (handler.PlatformView is not Android.Widget.EditText editText)
                return;

            editText.Background = new ColorDrawable(Android.Graphics.Color.Transparent);

            if (view is Editor { BackgroundColor: not null } mauiEntry)
            {
                editText.SetBackgroundColor(mauiEntry.BackgroundColor.ToPlatform());
            }
        });
    }
}