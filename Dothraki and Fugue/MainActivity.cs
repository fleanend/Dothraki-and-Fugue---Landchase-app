using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Content.Res;
using System.IO;
using Android.Util;
using Java.Util;
using System.Collections.Generic;

namespace Dothraki_and_Fugue {
   [Activity(Label = "Dothraki_and_Fugue", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]


    public class MainActivity : Activity {
        protected override void OnCreate( Bundle bundle ) {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            imageView.SetScaleType(ImageView.ScaleType.FitCenter);
            // foreach ();
            //imageView.SetImageResource(Resource.Drawable.Highgarden);
            AssetManager assets = this.Assets;
            string[] content;
            content = assets.List("Cards");
            List<Card> Cards = new List<Card>();// = ArrayList<Card>
            foreach (string c in content)
            { 
                Cards.Add(Card.myRegex(c));
            }
            
            foreach (Card c in Cards)
            {
                //multiple a change of seasons
                if (c.name == "A Change of Seasons")
                {
                    Log.Info("Patsgnac", "a change");
                }
                //tie day and night
                if (c.isTransforming())
                {
                    TransformingPlane c_T = (TransformingPlane) c;
                    foreach (Card d in Cards)
                    {
                        TransformingPlane d_T;
                        if (d.isTransforming())
                            d_T = (TransformingPlane)d;
                        else
                            break;
                        if (c_T.sameId(d_T))
                        {
                            c_T.link(d_T);
                            Log.Info("linked", c_T.name + " " + d_T.name);
                            break;
                        }
                    }
                    Log.Info("Patsgnac", "speriamo che ce lo mandi buono");
                }
            }
            
        }
    }
}

