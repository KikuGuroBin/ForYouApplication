using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

[assembly:ExportRenderer(typeof(MyContentPage), typeof(MyContentPageRenderer))]
namespace ForYouApplication.Droid
{
    public class MyContentPageRenderer : PageRenderer
    {
        /* 最初にタッチした座標 */
        private float DownX = -1;

        /* ドラッグ中の座標 */
        private float DrugX;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            Touch += OnTouch;
        }

        private void OnTouch(object sender, TouchEventArgs args)
        {
            var view = sender as Android.Views.View;

            /* コールバック用変数 */
            int action = -1;
            bool flag = false;
            float swipe = -1;

            /* アクションよって処理する */
            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    /* タッチしたX座標を保存 */
                    DownX = args.Event.GetX();
                
                    action = 1;

                    break;
                case MotionEventActions.Move:
                    /* 座標取得 */
                    float x = args.Event.GetX();

                    /* 画面外からスライドしてきた場合 */
                    if (DownX < 0)
                    {
                        DownX = x;
                    }

                    swipe = DownX - x;
                    
                    if ((DownX > view.Width - 10) && swipe > 0)
                    {
                        flag = true;
                    }
                    
                    action = 2;
                    
                    break;
                case MotionEventActions.Up:
                    /* 各変数の初期化 */
                    DownX = -1;
                    DrugX = -1;
                    action = 3;

                    break;
            }

            /* コールバック */
            MyContentPage page = Element as MyContentPage;
            page.Event(page, new MyContentEventArgs(action, flag, swipe));
        }

        private async void SwipeTimer()
        {
            await Task.Run(() =>
            {

            });
        }
    }
}