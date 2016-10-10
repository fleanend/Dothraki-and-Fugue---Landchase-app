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
   public class TransformingPlane : Plane
    {
        public int id;
        public bool dOrN;
        public TransformingPlane d;
        public TransformingPlane n;
        public void link(TransformingPlane other)
        {
            if (dOrN)
            {
                n = other;
                other.d = this;
            }
            else
            {
                d = other;
                other.n = this;
            }
        }
        public bool sameId(TransformingPlane other)
        {
            return this.id == other.id && this.name != other.name;
        }


        public TransformingPlane(string path, string name, int id, bool dOrN) : base(path, name)
        {
            this.id = id;
            this.dOrN = dOrN;
            if (dOrN)
            {
                d = this;
                n = null;
            }
            else
            {
                d = null;
                n = this;
            }
        }
        public override bool isTransforming()
        {
            return true;
        }
    }
}