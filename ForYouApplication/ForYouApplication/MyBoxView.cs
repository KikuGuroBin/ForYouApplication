using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ForYouApplication
{
    public class MyBoxView : BoxView
    {
        /* イベントハンドラ */
        public EventHandler<MyBoxViewEventArgs> MyEvent;

        /* ビュー本体の色 */
        public Color MainColor { get; set; }

        /* 角丸のサイズ */
        public int Radius { get; set; }

        /* 影の幅 */
        public int ShadowSize { get; set; }

        /* 影の有り無し */
        public bool ShadowFlag { get; set; }

        public MyBoxView()
        {
            MyEvent = (s, e) => { };
        }
    }
}
