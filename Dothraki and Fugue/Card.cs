using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace Dothraki_and_Fugue
{
    public class Card
    {
        public string Name;
        public string Path;

        public Android.Graphics.Drawables.BitmapDrawable Bm;

        public Card(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }

        public Card(Card card)
        {
            this.Path = card.Path;
            this.Name = card.Name;
        }

        public static Card MyRegex(string path)
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
                    if (path == "ph_A Change of Season")
                        return new Phenomenon(path, path.Replace(".png", "").Substring(3), true);
                    return new Phenomenon(path, path.Replace(".png", "").Substring(3));
                }
                else
                {
                    return new Plane(path, path.Replace(".png", ""));
                }
            }
                
        }

        public virtual bool IsTransforming()
        {
            return false;
        }

        ~Card()
        {

        }
    }

}