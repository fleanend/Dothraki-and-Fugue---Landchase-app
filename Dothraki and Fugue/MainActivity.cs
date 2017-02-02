using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Content.Res;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;
using Android.Views;
using UK.CO.Senab.Photoview;
//using Android.Locations.GpsStatus;

namespace Dothraki_and_Fugue {
    [Activity(Label = "Dothraki and Fugue", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]


    public class MainActivity : Activity, GestureDetector.IOnGestureListener
    {
        GestureDetector _gestureDetector;
        Card _currentPlane;
        ImageView _currentPlaneView;
        ImageButton _transformButton;
        ImageButton _seasonButton;
        ImageButton _toolTipButton;
        AssetManager _assets;
        Dialog _dialog;
        List<Card> _cards;
        int _index = 0;
        int _done = 0;
        int _currentSeason = 0;
        int _wondersCount = 0;
        int _lastSeasonIndex = 0;

        private PhotoViewAttacher _attacher;

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
            _toolTipButton = FindViewById<ImageButton>(Resource.Id.imageButton3);
            _gestureDetector = new GestureDetector(this);
           
            if (_done == 1)
                return;

            string[] toolTips = new string[content.Length];

            using (System.IO.StreamReader sr = new System.IO.StreamReader(_assets.Open("Tooltip.txt")))
            {
                // Read the stream to a string, and write the string to the console.
                string line = sr.ReadToEnd();
                toolTips = line.Split('#');
            }

            System.Console.WriteLine("" +toolTips.Length + toolTips[0]);

            int k = 0;
            foreach (string c in content)
            {
                Card cur = Card.MyRegex(c);
                if (k < toolTips.Length)
                     cur.SetToolTip(toolTips[k++]);
                _cards.Add(cur);
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
                    _cards.Add(new Phenomenon(_cards[i], true));
                    _cards.Add(new Phenomenon(_cards[i], true));
                    _cards.Add(new Phenomenon(_cards[i], true));
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
            _currentPlane.visited = true;
            _currentPlaneView.SetImageDrawable(_currentPlane.Bm);
            _attacher = new PhotoViewAttacher(_currentPlaneView);

            _attacher.SingleFling += ( sender, e ) => {
                OnFling(e.P0, e.P1, e.P2, e.P3);
            };

            _attacher.Update();

            System.Random rnd = new System.Random();
            _currentSeason = rnd.Next(0,4);

            _seasonButton.SetBackgroundColor(Color.Transparent);
            _transformButton.SetBackgroundColor(Color.Transparent);
            _toolTipButton.SetBackgroundColor(Color.Transparent);
            ChangeSeasons(_currentSeason);
            _seasonButton.Click += (sender, e) => {
                       ChangeSeasons(_currentSeason);
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

            if (_currentPlane.IsWonders())
            {
                _wondersCount = 1;
                _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_1);
            }
            else
            {
                _toolTipButton.SetImageResource(Resource.Drawable.tooltip_0);
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

            _toolTipButton.Click += (sender, e) =>  {
                _dialog = new Dialog(this);
                _dialog.SetContentView(Resource.Layout.secondlayout);

                string toolTip = _currentPlane.ToolTip;

                _dialog.SetTitle(toolTip.Substring(0,toolTip.IndexOf('@')));
                
                _dialog.SetCancelable(true);
                TextView text = _dialog.FindViewById<TextView>(Resource.Id.DialogView);
                text.Text = toolTip.Replace('@',' ');
                //Button CloseButton = _dialog.FindViewById<Button>(Resource.Id.DialogButton);
                //CloseButton.Click += DialogButtonClick;

                _dialog.Show();

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

            _currentSeason = (_currentSeason + 1) % 4;
            if (season == 0)
                _seasonButton.SetImageResource(Resource.Drawable.Spring);
            else if (season == 1)
                _seasonButton.SetImageResource(Resource.Drawable.Summer);
            else if (season == 2)
                _seasonButton.SetImageResource(Resource.Drawable.Autumn);
            else
                _seasonButton.SetImageResource(Resource.Drawable.Winter);
        }
        //change
        /*public void DialogButtonClick(object sender, System.EventArgs e)
        {
            Button rb = (Button)sender;
            _dialog.Dismiss();
            //Toast.MakeText(this, rb.Text, ToastLength.Short).Show();
        }*/


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

            var t = _currentPlane as Phenomenon;
            // Log.Debug("TEST", "About to change season");
            if (t!= null && t.IsChangeOfSeason && _index > _lastSeasonIndex)
            {
                Log.Debug("TEST", "Changing season");
                ChangeSeasons(_currentSeason);
                _lastSeasonIndex = _index;
            }

            if(_cards[_index].IsTransforming())
            {
                _transformButton.Visibility = ViewStates.Visible;
                _transformButton.SetImageResource(Resource.Drawable.d);
            }
            else
            {
                _transformButton.Visibility = ViewStates.Invisible;
            }

            if(!_cards[_index].IsWonders())
            {
                if (_wondersCount == 1)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_1);
                if (_wondersCount == 2)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_2);
                if (_wondersCount == 3)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_3);
                if (_wondersCount == 4)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_4);
                if (_wondersCount == 5)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_5);
                if (_wondersCount == 6)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_6);
                if (_wondersCount == 7)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_7);
                if (_wondersCount == 8)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_8);
                if (_wondersCount == 9)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_9);
            }
            else
            {
                if (!_cards[_index].visited)
                    _wondersCount++;
                if (_wondersCount == 1)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_1);
                if (_wondersCount == 2)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_2);
                if (_wondersCount == 3)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_3);
                if (_wondersCount == 4)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_4);
                if (_wondersCount == 5)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_5);
                if (_wondersCount == 6)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_6);
                if (_wondersCount == 7)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_7);
                if (_wondersCount == 8)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_8);
                if (_wondersCount == 9)
                    _toolTipButton.SetImageResource(Resource.Drawable.tooltip_w_9);
            }
            _currentPlane.visited = true;
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

