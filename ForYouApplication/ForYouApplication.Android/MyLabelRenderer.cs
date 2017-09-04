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

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using ForYouApplication;
using ForYouApplication.Droid;

[assembly:ExportRenderer(typeof(MyLabel), typeof(MyLabelRenderer))]
namespace ForYouApplication.Droid
{
    public class MyLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Touch += OnTouch;
        }

        private void OnTouch(object sender, TouchEventArgs args)
        {
            /* アクションの種別 */
            int action = 0;

            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    action = 1;
                    break;
                case MotionEventActions.Up:
                    action = 2;
                    break;
            }

            /* コールバック */
            MyLabel label = Element as MyLabel;
            label.Touch(label, new MyLabelEventArgs(action));
        }
    }
}