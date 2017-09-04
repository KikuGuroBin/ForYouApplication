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
        /* クリック検知用 */
        bool TouchFlag;

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            Touch += OnTouch;
        }

        public override void Draw(Canvas canvas)
        {
            MyBoxView view = (MyBoxView)Element;

            using (Paint paint = new Paint())
            {
                int shadowSize = view.ShadowSize;
                int blur = shadowSize;
                int radius = view.Radius;

                if (view.ShadowFlag)
                {
                    /* 影の描画 */
                    paint.Color = (Xamarin.Forms.Color.FromRgba(0, 0, 0, 30)).ToAndroid();
                    paint.SetMaskFilter(new BlurMaskFilter(blur, BlurMaskFilter.Blur.Normal));
                    RectF src = new RectF(0, shadowSize, Width - shadowSize * 2, Height - shadowSize);
                    canvas.DrawRoundRect(src, radius, radius, paint);
                }

                paint.AntiAlias = true;

                /* 本体の描画 */
                paint.Color = view.MainColor.ToAndroid();
                paint.SetMaskFilter(null);
                RectF vrc = new RectF(0, 0, Width - shadowSize * 2, Height - shadowSize * 2);
                canvas.DrawRoundRect(vrc, radius, radius, paint);
            }
        }

        private void OnTouch(object sender, TouchEventArgs args)
        {
            int action = -1;
            bool flag = false;

            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    action = 1;
                    TouchFlag = true;

                    break;
                case MotionEventActions.Up:
                    action = 2;

                    if (TouchFlag)
                    {
                        flag = true;
                        TouchFlag = false;
                    }

                    break;
            }

            MyBoxView view = Element as MyBoxView;
            view.MyEvent(view, new MyBoxViewEventArgs(action, flag));
        }
    }
}