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
using ForYouApplication;
using Xamarin.Forms;
using ForYouApplication.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(MyBoxView), typeof(MyBoxViewRenderer))]
namespace ForYouApplication.Droid
{
    public class MyBoxViewRenderer : BoxRenderer
    {
        public override void Draw(Canvas canvas)
        {
            MyBoxView myBoxView = (MyBoxView)Element;

            using (Paint paint = new Paint())
            {
                int shadowSize = myBoxView.ShadowSize;
                int blur = shadowSize;
                int radius = myBoxView.Radius;

                paint.AntiAlias = true;

                /* 本体の描画 */
                paint.Color = Android.Graphics.Color.Red;
                paint.SetMaskFilter(null);
                RectF rectangle = new RectF(0, 0, Width - shadowSize * 2, Height - shadowSize * 2);
                canvas.DrawRoundRect(rectangle, radius, radius, paint);
            }
        }
    }
}