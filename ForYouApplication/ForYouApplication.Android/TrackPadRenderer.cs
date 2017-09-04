using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

using ForYouApplication.Droid;
using ForYouApplication;
using Android.Views;
using System;
using Android.Graphics;

[assembly: ExportRenderer(typeof(TrackPad), typeof(TrackPadRenderer))]
namespace ForYouApplication.Droid
{
    public class TrackPadRenderer : BoxRenderer
    {
        /* 初期の相対位置 */
        float InitializationX;
        float InitializationY;

        /* 前回のイベント時の絶対位置 */
        float PreviousBeforeX;
        float PreviousBeforeY;

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            Touch += OnTrackPadTouch;
        }

        /* タッチ用イベント */
        private void OnTrackPadTouch(object sender, TouchEventArgs args)
        {
            double dx = 0;
            double dy = 0;
            int action = 1;

            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    /* 初期の相対値を保存 */
                    InitializationX = args.Event.GetX();
                    InitializationY = args.Event.GetY();

                    action = 1;

                    break;
                case MotionEventActions.Move:
                    /* 移動距離を計算 */
                    dx = (args.Event.RawX - PreviousBeforeX) / 1.7;
                    dy = (args.Event.RawY - PreviousBeforeY) / 1.7;

                    action = 2;

                    break;
                case MotionEventActions.Up:
                    action = 3;

                    break;
            }

            /* コールバック呼び出し */
            TrackPad el = Element as TrackPad;
            el.OnManipulationDelta(el, new ManipulationDeltaRoutedEventArgs(sender, dx, dy, action));
            
            /* 現在の絶対位置を保存 */
            PreviousBeforeX = args.Event.RawX;
            PreviousBeforeY = args.Event.RawY;
        }
    }
}