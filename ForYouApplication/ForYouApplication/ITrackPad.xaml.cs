using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ITrackPad : ContentPage
	{
        /* トラックパッドのポインタがタッチされているか */
        private bool TouchFlag;

        /* トラックパッドのポインタの座標の初期値 */
        private double InitTrackPadX;
        private double InitTrackPadY;

        /* トラックパッドのポインタの動かす前の座標 */
        private double SaveTrackPadX;
        private double SaveTrackPadY;

        /* リモートホストとの通信用クライアント */
        private AsyncTcpClient Client;

        /* デバッグ用コンストラクタ */
		public ITrackPad ()
		{
			InitializeComponent ();
		}

        public ITrackPad(AsyncTcpClient client)
        {
            InitializeComponent();

            Client = client;

            /* トラックパッドのポインタをレイアウトの中心に */
            TrackPad.VerticalOptions = LayoutOptions.Center;
            TrackPad.HorizontalOptions = LayoutOptions.Center;
        }

        /* 画面サイズが変更されたときのイベント */
        protected override void OnSizeAllocated(double width, double heigth)
        {
            /* トラックパッドのポインタの初期値を格納 */
            InitTrackPadX = TrackPad.X + TrackPad.Width / 2;
            InitTrackPadY = TrackPad.Y + TrackPad.Height / 2;

            /*  */
            SaveTrackPadX = InitTrackPadX;
            SaveTrackPadY = InitTrackPadY;
        }

        /* トラックパッドのポインタが動くのを検知する */
        private async void TrackPadMove()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    /* 座標の差分を計算 */
                    double x = TrackPad.X + TrackPad.Width / 2 - SaveTrackPadX;
                    double y = TrackPad.Y + TrackPad.Height / 2 - SaveTrackPadY;

                    /* トラックパッドのポインタの中心座標が移動した場合 */
                    if (x != 0 || y != 0)
                    {
                        /* リモートホストに移動した分の数値を送信 
                        Client.Send(TextProcess.TextJoin(null, TagConstants.MOUSE.GetConstants(), x.ToString(), y.ToString()));
                        */

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TrackPad.TranslationX = InitTrackPadX;
                            TrackPad.TranslationY = InitTrackPadY;
                        });
                    }
                }
            });
        }
    }
}