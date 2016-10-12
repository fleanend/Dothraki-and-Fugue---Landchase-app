using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Content.Res;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views;

namespace Dothraki_and_Fugue {
    [Activity(Label = "Dothraki_and_Fugue", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]


    public class MainActivity : Activity, GestureDetector.IOnGestureListener
    {
        GestureDetector _gestureDetector;
        Card _currentPlane;
        ImageView _currentPlaneView;
        ImageButton _transformButton;
        ImageButton _seasonButton;
        AssetManager _assets;
        List<Card> _cards;
        int _index = 0;
        int _done = 0;
        int _season = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _assets = this.Assets;
            var content = _assets.List("Cards");
            _cards = new List<Card>();// = ArrayList<Card>
            _currentPlaneView = FindViewById<ImageView>(Resource.Id.imageView1);
            _transformButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            _seasonButton = FindViewById<ImageButton>(Resource.Id.imageButton2);
            _gestureDetector = new GestureDetector(this);

            if (_done == 1)
                return;

            foreach (string c in content)
            {
                _cards.Add(Card.MyRegex(c));
            }

            int doOnce = 0;


            // get input stream
            System.IO.Stream ims = null;
            for (int i = 0; i < _cards.Count; i++)
            {
                //multiple a change of seasons
                ims = _assets.Open("Cards/" + _cards[i].Path);
                // load image as Drawable
                _cards[i].Bm =  new BitmapDrawable(BitmapFactory.DecodeStream(ims));
                if (_cards[i].Name == "A Change of Seasons" && doOnce == 0)
                {
                    doOnce++;
                    _cards.Add(new Card(_cards[i]));
                    _cards.Add(new Card(_cards[i]));
                    _cards.Add(new Card(_cards[i]));
                }
                //tie day and night
                if (_cards[i] is TransformingPlane)
                {
                    TransformingPlane c_T = (TransformingPlane)_cards[i];
                    for (int j = i + 1; j < _cards.Count; j++)
                    {
                        TransformingPlane d_T;
                        if (_cards[j].IsTransforming())
                        { 
                            d_T = (TransformingPlane)_cards[j];
                            ims = _assets.Open("Cards/" + _cards[j].Path);
                            _cards[j].Bm = new BitmapDrawable(BitmapFactory.DecodeStream(ims));
                        }
                        else
                            continue;

                        if (c_T.SameId(d_T))
                        {
                            c_T.Link(d_T);
                            if (c_T.DorN)
                                _cards.Remove(_cards[j]);
                            else
                                _cards.Remove(_cards[i]);
                            continue;
                        }
                    }
                }
            }
            _cards = Randomize(_cards);

            
            // set image to ImageView
            
            _currentPlaneView.SetScaleType(ImageView.ScaleType.FitCenter);
            _currentPlane = _cards[0];
            _currentPlaneView.SetImageDrawable(_currentPlane.Bm);

            System.Random rnd = new System.Random();
            _season = rnd.Next(0,4);

            _seasonButton.SetBackgroundColor(Color.Transparent);
            ChangeSeasons(_season);
            _seasonButton.Click += (sender, e) => {
                _season = (_season + 1)%4;
                ChangeSeasons(_season);
            };

            if (_cards[_index].IsTransforming())
            {
                _transformButton.Visibility = ViewStates.Visible;
                _transformButton.SetImageResource(Resource.Drawable.d);
            }
            else
            {
                _transformButton.Visibility = ViewStates.Invisible;
            }
            _transformButton.Click += (sender, e) => {
                
                //Nuovo Current Plane
                _currentPlane = ((TransformingPlane) _currentPlane).ManageToggle();
                //Modifica icona del pulsante
                _transformButton.SetImageResource(((TransformingPlane) _currentPlane).DorN
                    ? Resource.Drawable.d
                    : Resource.Drawable.n);
                _currentPlaneView.SetImageDrawable(null);
                //Visualizzazione nuovo piano
                _currentPlaneView.SetImageDrawable(_currentPlane.Bm);
            };
            _done = 1;

        }

        public static List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            System.Random rnd = new System.Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }

        public void ChangeSeasons(int season)
        {
            if (season == 0)
                _seasonButton.SetImageResource(Resource.Drawable.Spring);
            else if (season == 1)
                _seasonButton.SetImageResource(Resource.Drawable.Summer);
            else if (season == 2)
                _seasonButton.SetImageResource(Resource.Drawable.Autumn);
            else
                _seasonButton.SetImageResource(Resource.Drawable.Winter);
        }

        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
           if(e1.RawX-e2.RawX < 0)
            {
                if (_index > 0)
                    _index--;
               
            }
           else
            {
                if (_index < _cards.Count-1)
                    _index++;
            }
            _currentPlane = _cards[_index];
            _currentPlaneView.SetImageDrawable(_currentPlane.Bm);

            if(_cards[_index].IsTransforming())
            {
                _transformButton.Visibility = ViewStates.Visible;
                _transformButton.SetImageResource(Resource.Drawable.d);
            }
            else
            {
                _transformButton.Visibility = ViewStates.Invisible;
            }
            return true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _currentPlane = null;
            _currentPlaneView.SetImageDrawable(null);
        }

        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return false;
        }
    }

}

