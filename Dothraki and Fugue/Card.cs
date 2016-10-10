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
using Android.Util;

namespace Dothraki_and_Fugue
{
    public class Card
    {
        public string name;
        public string path;

        public Card(string path, string name)
        {
            this.path = path;
            this.name = name;
        }

        public Card(Card card)
        {
            this.path = card.path;
            this.name = card.name;
        }

        public static Card myRegex(string path)
        {
            char[] cName = path.ToCharArray();
            if (cName[0] == 'd' || cName[0] == 'n')
            {
                int i = 1;
                int id = 0;
                while(Char.IsDigit(cName[i]))
                {
                    id = id*10 + int.Parse(cName[i].ToString());
                    i++;
                    if (i == path.Length)
                        break;
                    if( cName[i] == '_')
                    {
                        if (cName[0] == 'd')
                        {
                            return new TransformingPlane(path, path.Substring(i+1).Replace(".png", ""),id, true);
                        }
                        else
                        {
                            return new TransformingPlane(path, path.Substring(i+1).Replace(".png", ""), id, false);
                        }
                    }
                }
                return new Plane(path, path.Replace(".png", ""));
            }
            else
            {
                if (cName[0] == 'p' && cName[1] == 'h' && cName[2] == '_')
                {
                    return new Phenomenon(path, path.Replace(".png", "").Substring(3));
                }
                else
                {
                    return new Plane(path, path.Replace(".png", ""));
                }
            }
                
        }

        public virtual bool isTransforming()
        {
            return false;
        }

        ~Card()
        {

        }
    }

}