using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dothraki_and_Fugue
{
    class Phenomenon : Card
    {
        public Phenomenon(string path, string name) : base(path, name)
        {
        }
        public Phenomenon(Card card) : base(card)
        {
        }
        public override bool isTransforming()
        {
            return false;
        }
    }
}