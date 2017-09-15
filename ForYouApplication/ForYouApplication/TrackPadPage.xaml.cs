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

        /*中ホイールon/offのｆラッグ*/
        bool WheelFlag=false;
        
        /* デバッグ用コンストラクタ */
		public TrackPadPage ()
		{
			InitializeComponent ();
            
            /* イベント設定 */
            TrackPad.OnManipulationDelta += TrackPadDrug;
            Left.Touch += OnLabelTouch;
            Right.Touch += OnLabelTouch;
        }

        public TrackPadPage(AsyncTcpClient client)
        {
            InitializeComponent();

            Client = client;
            
            /* イベント設定 */
            TrackPad.OnManipulationDelta += TrackPadDrug;
            Left.Touch += OnLabelTouch;
            Right.Touch += OnLabelTouch;
        }

        /* 画面サイズが変更された時のイベント */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            
            MainPane.WidthRequest = width;
            
            ClickPane.TranslationY = MainPane.Height - ClickPane.Height;
            ClickPane.WidthRequest = width;
            
            /* 中心座標 */
            double tpX = width / 2 - TrackPad.Width / 2;
            double tpY = (height - ClickPane.Height) / 2 - TrackPad.Height / 2;

            /*
            Rectangle trc = TrackPad.Bounds;
            trc.X = tpX;
            trc.Y = tpY;
            TrackPad.LayoutTo(trc);
            */

            TrackPad.TranslationX = tpX;
            TrackPad.TranslationY = tpY;

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
                /* ダウン */
                action = TagConstants.MDOWN.GetConstants();

                /* ポインタの画像を差し替える */
                PointerImage.Source = "Mouse_On.gif";
            }
            else if (args.Action == 2)
            {
                /* ドラッグ */
                action = TagConstants.MOVE.GetConstants();

                /* ポインタの画像を移動 */
                PointerImage.TranslationX += args.Translation.X;
                PointerImage.TranslationY += args.Translation.Y;

                if (!DrugFlag)
                {
                    DrugFlag = true;
                }
            }
            else if (args.Action == 3)
            {
                /* アップ */
                action = TagConstants.MUP.GetConstants();

                /* ポインタの画像を差し替える */
                PointerImage.Source = "Mouse_Off.gif";

                /* ドラッグからのアップだった場合 */
                if (DrugFlag)
                {
                    DrugFlag = false;

                    /* ポインタと画像を中心に移動 */
                    Rectangle trc = new Rectangle(0, 0, 60, 60);
                    TrackPad.LayoutTo(trc);

                    PointerImage.TranslationX = InitX;
                    PointerImage.TranslationY = InitY;
                }
            }
            /* 移動した数値をリモートホスト側の処理用に加工 */
            double x = Math.Truncate(args.Translation.X);
            double y = Math.Truncate(args.Translation.Y);

            if (args.Action == 2 && x == 0 && y == 0)
            {
                return;
            }

            /* 加工 */
            string send = TextProcess.TextJoin(null, action, x.ToString(), y.ToString());

            /* 送信 */
            if (Client != null)
            {
                System.Diagnostics.Debug.WriteLine("deg : action = " + action + "x = " + x + "y = " + y);
                Client.Send(send);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("deg : action = " + action);
            }
        }

        /* 左クリックまたは右クリック時のイベント */
        private void OnLabelTouch(object sender, MyLabelEventArgs args)
        {
            if (!DrugFlag && Client != null)
            {
                /* タグ生成 */
                string tag = CreateTag(sender, args.Action);

                if (tag != null)
                {
                    /* リモートホスト側が処理する形式に加工 */
                    string send = TextProcess.TextJoin(null, tag);

                    /* 送信 */
                    Client.Send(send);
                }
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

        private void OnWheelClick(object sender, EventArgs args)
        {
            if (!DrugFlag && Client != null)
            {
                if (!WheelFlag) {
                    string send = TextProcess.TextJoin(null, "<CCD>");
                    Client.Send(send);
                    WheelFlag = true;
                }
                else
                {
                    string send = TextProcess.TextJoin(null, "<CCU>");
                    Client.Send(send);
                    WheelFlag = false;
                }

            }
        }

        public void SetClient(AsyncTcpClient client)
        {
            Client = client;
        }

        /* 送信タグ生成 */
        private string CreateTag(object sender, int action)
        {
            StringBuilder tag = new StringBuilder("<");

            /* 引数senderの判別 */
            if (sender.Equals(Right))
            {
                tag.Append("RC");
            }
            else
            {
                tag.Append("LC");
            }

            /* アクションの種類 */
            switch (action)
            {
                /* ダウン */
                case 1:
                    tag.Append("D>");
                    break;
                /* アップ */
                case 2:
                    tag.Append("U>");
                    break;
                default:
                    return null;
            }

            return tag.ToString();
        }
    }
}