using Android.App;
using Android.Widget;
using Android.OS;

namespace Dothraki_and_Fugue {
    [Activity(Label = "Dothraki_and_Fugue", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        protected override void OnCreate( Bundle bundle ) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

