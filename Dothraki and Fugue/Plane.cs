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
    public class Plane : Card
    {
        public Plane(string path, string name) : base(path, name)
        {
        }
        public override bool isTransforming()
        {
            return false;
        }
    }
}