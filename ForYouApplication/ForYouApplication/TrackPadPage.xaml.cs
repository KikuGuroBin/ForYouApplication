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
	public partial class TrackPadPage : ContentPage
	{
        /* リモートホストとの通信用 */
        private AsyncTcpClient Client;

        /* ドラッグの判定 */
        private bool DrugFlag;

        /* トラックパッドのポインタの初期座標 */
        private double InitX;
        private double InitY;
        
        private bool LeftFlag;
        private bool RightFlag;

        /* デバッグ用コンストラクタ */
		public TrackPadPage ()
		{
			InitializeComponent ();

            TrackPad.SetEvent();

            TrackPad.OnManipulationDelta += TrackPadDrug;
        }

        public TrackPadPage(AsyncTcpClient client)
        {
            InitializeComponent();

            Client = client;

            TrackPad.SetEvent();

            TrackPad.OnManipulationDelta += TrackPadDrug;
        }

        /* 画面サイズが変更された時のイベント */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            MainPane.WidthRequest = width;
            
            ClickPane.TranslationY = MainPane.Height - ClickPane.Height;
            ClickPane.WidthRequest = width;
            
            double tpX = width / 2 - TrackPad.Width / 2;
            double tpY = (height - ClickPane.Height) / 2 - TrackPad.Height / 2;
            
            Rectangle trc = TrackPad.Bounds;
            trc.X = tpX;
            trc.Y = tpY;
            TrackPad.LayoutTo(trc);

            PointerImage.TranslationX = tpX;
            PointerImage.TranslationY = tpY;

            /* トラックパッドのポインタの座標の初期値を格納 */
            InitX = tpX;
            InitY = tpY;
        }

        /* トラックパッドのポインタをドラッグした時のイベント */
        private void TrackPadDrug(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            string action = null;

            /* 引数argsのフィールドによってリモートホストに送信するタグを変える */
            if (args.Action == 1)
            {
                action = TagConstants.MDOWN.GetConstants();

                PointerImage.Source = "Mouse_On.gif";
            }
            else if (args.Action == 2)
            {
                action = TagConstants.MOVE.GetConstants();

                PointerImage.TranslationX += args.Translation.X;
                PointerImage.TranslationY += args.Translation.Y;

                if (!DrugFlag)
                {
                    DrugFlag = true;
                }
            }
            else if (args.Action == 3)
            {
                action = TagConstants.MUP.GetConstants();

                PointerImage.Source = "Mouse_Off.gif";

                if (DrugFlag)
                {
                    DrugFlag = false;

                    Rectangle trc = TrackPad.Bounds;
                    trc.X = InitX;
                    trc.Y = InitY;
                    TrackPad.LayoutTo(trc);

                    PointerImage.TranslationX = InitX;
                    PointerImage.TranslationY = InitY;
                }
            }

            double x = args.Translation.X;
            double y = args.Translation.Y;

            string send = TextProcess.TextJoin(null, action, x.ToString(), y.ToString());

            if (Client != null)
            {
                Client.Send(send);
            }
        }

        private void OnLeftClick(object sender, EventArgs args)
        {
            if (!DrugFlag && Client != null)
            {
                string send = null;

                if (LeftFlag)
                {
                    send = TextProcess.TextJoin(null, "<LCU>");
                    LeftFlag = false;
                }
                else
                {
                    send = TextProcess.TextJoin(null, "<LCD>");
                    LeftFlag = true;
                }
                
                Client.Send(send);
            }
        }

        private void OnRightClick(object sender, EventArgs args)
        {

            if (!DrugFlag && Client != null)
            {
                string send = null;

                if (LeftFlag)
                {
                    send = TextProcess.TextJoin(null, "<RCU>");
                    RightFlag = false;
                }
                else
                {
                    send = TextProcess.TextJoin(null, "<RCD>");
                    RightFlag = true;
                }
               
                Client.Send(send);
            }
        }

        private void OnUpClick(object sender, EventArgs args)
        {
            if (!DrugFlag && Client != null)
            {
                string send = TextProcess.TextJoin(null, "<CCU>");
                Client.Send(send);
            }
        }

        private void OnDownClick(object sender, EventArgs args)
        {
            if (!DrugFlag && Client != null)
            {
                string send = TextProcess.TextJoin(null, "<CCD>");
                Client.Send(send);
            }
        }
    }
}