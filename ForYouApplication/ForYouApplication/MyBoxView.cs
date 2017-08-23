using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ForYouApplication
{
    public class MyBoxView : BoxView
    {
        /* 角丸のサイズ */
        public int Radius { get; set; }

        /* 影の幅 */
        public int ShadowSize { get; set; }

        public MyBoxView()
        {
            Radius = 100;
            ShadowSize = 5;
        }
    }
}
