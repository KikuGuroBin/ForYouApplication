using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

using ForYouApplication.Droid;
using ForYouApplication;
using Android.Views;

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
            Android.Views.View view = sender as Android.Views.View;
            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    /* 初期の相対値を保存 */
                    InitializationX = args.Event.GetX();
                    InitializationY = args.Event.GetY();

                    break;
                case MotionEventActions.Move:
                    /* 移動距離を計算 */
                    float dx = args.Event.RawX - PreviousBeforeX;
                    float dy = args.Event.RawY - PreviousBeforeY;
                    
                    /* 
                     * コールバック呼び出し
                     * TODO: delta 方式なのか誤差が大きい 
                     */
                    TrackPad el = Element as TrackPad;
                    el.OnManipulationDelta(el, new ManipulationDeltaRoutedEventArgs(sender, dx, dy));

                    break;
                case MotionEventActions.Up:
                    break;
            }

            /* 現在の絶対位置を保存 */
            PreviousBeforeX = args.Event.RawX;
            PreviousBeforeY = args.Event.RawY;
        }
    }
}