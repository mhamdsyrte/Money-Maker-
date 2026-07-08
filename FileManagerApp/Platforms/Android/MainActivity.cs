using Android.App;
using Android.Content.PM;
using Android.OS;

namespace FileManagerApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // Request storage permissions for Android 11+
        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            if (!Android.OS.Environment.IsExternalStorageManager)
            {
                try
                {
                    var intent = new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                    StartActivity(intent);
                }
                catch { }
            }
        }
    }
}
