using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace App1
{
    [Activity(Label = "Dothraki and Fugue", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Verbose("prova","Prova");
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

