using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace Dothraki_and_Fugue {
   [Activity(Label = "Dothraki_and_Fugue", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity {
        protected override void OnCreate( Bundle bundle ) {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            imageView.SetScaleType(ImageView.ScaleType.FitCenter);
            imageView.SetImageResource(Resource.Drawable.Highgarden);

        }
    }
}

