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
            
        }

        /* 画面サイズが変更されたときのイベント */
        protected override void OnSizeAllocated(double width, double heigth)
        {
            /* トラックパッドのポインタの初期値を格納 */
            InitTrackPadX = TrackPad.X;
            InitTrackPadY = TrackPad.Y;
        }

        /* レンダラーのコールバック */
        private void OnDrug(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            string action = null;

            /* アクションごとに処理 */
            switch (args.Action)
            {
                case 1:
                    action = TagConstants.DOWN.GetConstants();
                    break;
                case 2:
                    action = TagConstants.MOVE.GetConstants();
                    break;
                case 3:
                    action = TagConstants.UP.GetConstants();
                    break;
            }

            /* 加工 */
            string send = TextProcess.TextJoin(null, action, args.Translation.X.ToString(), args.Translation.Y.ToString());

            Client.Send(send);
        }
    }
}