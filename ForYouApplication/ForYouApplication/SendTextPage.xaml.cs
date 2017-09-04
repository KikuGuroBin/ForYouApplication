using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendTextPage : MyContentPage
	{
        /* リモートホストとの通信用 */
        private AsyncTcpClient Client;

        /* データバインディング用インスタンス */
        private LabelSize ArrowSize;
        private LabelSize SubKeySize;

        /* Editorの一時保存用 */
        private string SaveText;

        /* Editor.TextChange制御用 */
        private bool EditorFlag;

        /* OnSizeAllocated制御用 */
        private bool Order;

        /* Shiftモード判定 */
        private bool ShiftFlag;

        /* ショートカットパネルの表示状態 */
        private bool IsShortCutPaneShow;

        /* PCキー操作パネルの表示状態 */
        private bool IsPCKeyPaneShow;

        private bool Showing;
        
        private readonly Color LightYellow = Color.FromRgb(0xFF, 0xEF, 0x85);
        
        public SendTextPage ()
		{
            InitializeComponent();

            Opacity = 0;

            /* データバインディング */
            ShortCutList.BindingContext = new ShortCutListViewModel();
            
            /* イベント設定 */
            Event += PageTouch;
            ShortCutShow.MyEvent += ShortCutTouch;
            PCKeyShow.MyEvent += PCKeyTouch;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (!Order)
            {
                /* メインのレイアウトの幅、高さを設定 */
                SendItems.WidthRequest = width;
                SendItems.HeightRequest = height - 70;

                /* ラベルなどに設定する幅、高さを求める */
                double labelWidth = width / 8;
                double labelHeight = labelWidth / 2 * 1.5;

                /* 矢印キーのバインディング用インスタンス生成 */
                ArrowSize = new LabelSize(labelWidth, labelHeight);

                labelWidth = width / 6;

                SendEditor.HeightRequest = labelHeight * 2;

                /* PC操作キーのバインディング用インスタンス生成 */
                SubKeySize = new LabelSize(labelWidth, labelHeight);

                /* データバインディング */
                UpLabel.BindingContext = ArrowSize;
                LeftLabel.BindingContext = ArrowSize;
                DownLabel.BindingContext = ArrowSize;
                RightLabel.BindingContext = ArrowSize;
                Shift.BindingContext = SubKeySize;
                Tab.BindingContext = SubKeySize;
                BackLabel.BindingContext = SubKeySize;
                ChangeTab.BindingContext = SubKeySize;
                Enter.BindingContext = SubKeySize;

                /*
                SubKeyPane.WidthRequest = width;
                SubKeyPane.HeightRequest = labelHeight;
               
                Rectangle rc = ArrowPane.Bounds;
                rc.Width = width;
                rc.Height = labelHeight * 2 + 21;
                ArrowPane.LayoutTo(rc);

                MovingView(UpLabel, ArrowSize.Width + 14, 7, -1, -1);
                MovingView(LeftLabel, 7, ArrowSize.Height + 14, -1, -1);
                MovingView(DownLabel, ArrowSize.Width + 14, ArrowSize.Height + 14, -1, -1);
                MovingView(RightLabel, ArrowSize.Width * 2 + 21, ArrowSize.Height + 14, -1, -1);

                rc = ArrowPane.Bounds;
                rc.X = 7;
                rc.Width = Width;
                rc.Height = 0;
                ArrowPane.LayoutTo(rc);

                rc = SubKeyPane.Bounds;
                rc.X = 7;
                rc.Width = Width;
                rc.Height = labelHeight + 14;
                SubKeyPane.LayoutTo(rc);

                MovingView(Shift, 7, 7, -1, -1);
                MovingView(Tab, labelWidth + 14, 7, -1, -1);
                MovingView(ChangeTab, labelWidth * 2 + 21, 7, -1, -1);
                MovingView(BackLabel, labelWidth * 3 + 28, 7, -1, -1);
                MovingView(Enter, labelWidth * 4 + 35, 7, -1, -1);

                rc.Y = ArrowPane.Y + 7;
                rc.Height = 0;
                SubKeyPane.LayoutTo(rc);
                */

                /* PC操作キーパネルキー配置 */
                MovingView(PCKeyPane, -1, -1, width, labelHeight * 4 + 41);
                MovingView(Tab, 10, 10, -1, -1);
                MovingView(ChangeTab, 20, Tab.Height + 17, -1, -1);
                MovingView(Shift, 30, ChangeTab.Height + Tab.Height + 27, -1, -1);
                MovingView(BackLabel, width - labelWidth - 10, 10, -1, -1);
                MovingView(Enter, width - labelWidth - 20, labelHeight + 17, -1, -1);

                double x = width - Enter.Width - UpLabel.Width - 27;
                double y = BackLabel.Height + Enter.Height + 24;

                MovingView(UpLabel, x, y, -1, -1);
                MovingView(LeftLabel, x - LeftLabel.Width - 7, y + UpLabel.Height + 7, -1, -1);
                MovingView(DownLabel, x, y + UpLabel.Height + 7, -1, -1);
                MovingView(RightLabel, x + DownLabel.Width + 7, y + UpLabel.Height + 7, -1, -1);

                MovingView(PCKeyPane, -1, -1, -1, 0);

                /*
                FloatingPane.LayoutTo(new Rectangle(width - 160, height - 170, 160, 170));
                
                Rectangle rc = ShortCutShow.Bounds;
                rc.X = 80;
                rc.Y = 80;
                ShortCutShow.LayoutTo(rc);
                
                rc.X = 80;
                rc.Y = 0;
                SubKeyShow.LayoutTo(rc);

                rc.X = 0;
                rc.Y = 80;
                ArrowShow.LayoutTo(rc);
                */

                /* フローティングボタンもどきを配置 */
                MovingView(FloatingPane, width - 160, height - 170, 160, 170);
                MovingView(ShortCutShow, 80, 80, -1, -1);
                MovingView(PCKeyShow, 80, 0, -1, -1);
               
                /* ショートカットパネルの隠す */
                MovingView(ShortCutPane, width, 0, width / 3, height);
                
                /* 次回以降呼ばれないようにする */
                Order = false;

                Opacity = 1;
            }
        }

        /* ショートカットリストのアイテムが選択されたときのイベント */
        private void ShortCutListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Client == null)
            {
                return;
            }

            /* 選択されたリストアイテム取得 */
            ShortCutListItem item = e.SelectedItem as ShortCutListItem;

            /* アイテムのフィールドIDを取得 */
            int id = item?.Id ?? -1;

            if (id == -1)
            {
                return;
            }

            /* ショートカットコマンドの取得 */
            string send = item.Send;

            /* ショートカット送信 */
            Client.Send(send);

            ShortCutList.SelectedItem = null;
        }


        /* Editorのテキストが変更(入力、変換、削除)されたときのイベント */
        private void EditorTextChanged(object sender, EventArgs args)
        {
            /* TextChange中からのTextChange発生を抑制 */
            if (!EditorFlag)
            {
                /* テキスト送信とEditor.Text操作 */
                SendText(SendEditor.Text);
            }
        }

        /* シフトラベルタップ時のイベント */
        private void ShiftTap(object sender, EventArgs args)
        {
            /* シフトモードON時には使用できるラベルの背景色を変える */
            if (!ShiftFlag)
            {
                UpLabel.BackgroundColor = LightYellow;
                DownLabel.BackgroundColor = LightYellow;
                RightLabel.BackgroundColor = LightYellow;
                LeftLabel.BackgroundColor = LightYellow;
                Tab.BackgroundColor = LightYellow;
            }
            /* OFFにしたときに背景色を戻す */
            else
            {
                UpLabel.BackgroundColor = Color.White;
                DownLabel.BackgroundColor = Color.White;
                RightLabel.BackgroundColor = Color.White;
                LeftLabel.BackgroundColor = Color.White;
                Tab.BackgroundColor = Color.White;
            }

            ShiftFlag = !ShiftFlag;
        }

        /* ShortCutList表示時の背面のBoxViewをタップしたときのイベント 
        private async void BoxViewTap(object sender, EventArgs args)
        {
            Rectangle rc = new Rectangle(Width - Width / 3, 0, Width / 3, Height);
            await Task.Run(() =>
            {
                double move = (Width / 3) / 5;

                rc.X += move;

                while (Width > rc.X)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ShortCutPane.LayoutTo(rc);
                        Thread.Sleep(1);
                    });

                    rc.X += move;
                }
            });
        }
        */

        /* PC操作キータッチ時のイベント */
        private void SubKeyTap(object sender, EventArgs args)
        {
            if (Client == null)
            {
                return;
            }

            if (sender.Equals(BackLabel))
            {
                if (!ShiftFlag)
                {
                    Client.Send("<BAC>");
                }
            }
            else if (sender.Equals(UpLabel))
            {
                Client.Send("<UPP>");
            }
            else if (sender.Equals(RightLabel))
            {
                Client.Send("<RIG>");
            }
            else if (sender.Equals(DownLabel))
            {
                Client.Send("<DOW>");
            }
            else if (sender.Equals(LeftLabel))
            {
                Client.Send("<LEF>");
            }
            else if (sender.Equals(Shift))
            {
                Client.Send("<SHI>");
            }
            else if (sender.Equals(Tab))
            {
                Client.Send("<TAB>");
            }
            else if (sender.Equals(Enter))
            {
                Client.Send("<ENT>");
            }
            else if (sender.Equals(ChangeTab))
            {
                if (!ShiftFlag)
                {
                    Client.Send("<CTA>");
                }
            }
        }

        /* 
         * ページに対するモーション検知イベント
         * MyContentPageRendererからのコールバック 
         */
        private void PageTouch(object sender, MyContentEventArgs args)
        {
        }

        /* 
         * ショートカットボタン用タッチ時のイベント 
         * MyBoxViewRendererからのコールバック
         */
        private void ShortCutTouch(object sender, MyBoxViewEventArgs args)
        {
            if (args.TouchFlag && !Showing)
            {
                ShowShortCutPane();
            }
        }

        /* 
         * PCキーボタン用タッチイベント 
         * MyBoxViewRendererからのコールバック
         */
        private void PCKeyTouch(object sender, MyBoxViewEventArgs args)
        {
            if (args.TouchFlag)
            {
                ShowPCKeyPane();
            }

            /*
            if (args.TouchFlag)
            {
                if (!ArrowPaneShow)
                {
                    MovingView(SubKeyPane, -1, SubKeyPane.Y + ArrowPane.Height, -1, -1);
                    MovingView(ArrowPane, -1, -1, -1, ArrowSize.Height * 2 + 21);
                    MovingView(UpLabel, ArrowSize.Width + 14, 7, -1, -1);
                    MovingView(LeftLabel, 7, ArrowSize.Height + 14, -1, -1);
                    MovingView(DownLabel, ArrowSize.Width + 14, ArrowSize.Height + 14, -1, -1);
                    MovingView(RightLabel, ArrowSize.Width * 2 + 21, ArrowSize.Height + 14, -1, -1);
                }
                else
                {
                    MovingView(ArrowPane, -1, -1, -1, 0);
                    MovingView(SubKeyPane, -1, SubKeyPane.Y - ArrowPane.Height, -1, -1);
                }

                ArrowPaneShow = !ArrowPaneShow;
            }
            */
        }

        /* ショートカットラベルタップ用のイベント */
        private void ShortCutTap(object sender, EventArgs args)
        {
            if (!Showing)
            {
                ShowShortCutPane();
            }
        }
        
        /* バックボタン押下時のイベント */
        protected override bool OnBackButtonPressed()
        {
            /* 何もせずにtrueを返すことでバックキーを無効化 */
            return true;
        }

        /* 
         * リモートホストへの送信とEditor.Textの操作 
         * TextChangeイベント制御のため非同期処理
         */
        private async void SendText(string text)
        {
            /* テキストチェンジ開始 */
            EditorFlag = true;

            await Task.Run(() =>
            {
                try
                {
                    /* 加工後の文字列とEditorを操作するための数値を取得 */
                    (int index, string send) process = SaveText == null || SaveText == "" ?
                                                       (-1, text) : TextProcess.SendContentDeicsion(SaveText, text);

                    string send = process.send;

                    /* リモートホストへ送信 */
                    if (send == "\n")
                    {
                        /* 改行文字のみの入力の場合は改行用コマンドを送る */
                        Client.Send("<ENT>");
                    }
                    else
                    {
                        /* それ以外は普通に送信する */
                        Client.Send(send);
                    }
                    
                    /* クライアントが変換を行った場合 
                       TODO:イベントの性質上小文字、濁点などでも反応する

                    if (process.index > -1)
                    {
                        /* 保存する文字列を抽出 
                        string save = text.Substring(0, process.index + 1);

                        /* Editor.Textに残す文字を抽出 
                        text = text.Substring(process.index + 1);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            /* Editor.Text00更 
                            DetailPage.Editor.Text = text;
                        });

                        /* ログに保存する 
                        SendLog.Enqueue(save);
                    }

                    /* クライアントが改行した場合 */
                    if (text.IndexOf("\n") > -1)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            /* Editor.Textを空白にする */
                            SendEditor.Text = "";
                        });
                    }

                    /* 現在のEditorのテキストの内容を保存 */
                    SaveText = text;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("deg : SendTextPage.Send" + e);
                }

            });

            /* テキストチェンジ終了 */
            EditorFlag = false;
        }

        public void SetClient(AsyncTcpClient client)
        {
            Client = client;

            /* 受信待ち開始 */
            Client.Receive();
        }

        /* Viewを動かすメソッド */
        private async void MovingView(View view, params double[] bounds)
        {
            /* 引数viewの境界値取得 */
            Rectangle rc = view.Bounds;

            for (int i = 0; i < 4; i++)
            {
                double d = bounds[i];

                /* 引数boundsに-1以外が設定されているものだけ処理開始 */
                if (d > -1)
                {
                    /* 添え字ごとに設定するフィールドを変える */
                    switch (i)
                    {
                        case 0:
                            rc.X = d;
                            break;
                        case 1:
                            rc.Y = d;
                            break;
                        case 2:
                            rc.Width = d;
                            break;
                        case 3:
                            rc.Height = d;
                            break;
                    }
                }
            }

            /* 実行 */
            await view.LayoutTo(rc, 110);
        }

        private void ShowPCKeyPane()
        {
            Showing = true;

            /* パネルの表示非表示ごとに処理 */
            if (!IsPCKeyPaneShow)
            {
                double width = PCKeyPane.Width;
                
                /* パネル表示 */
                MovingView(PCKeyPane, -1, -1, width, SubKeySize.Height * 4 + 41);

                /* 矢印キー以外を配置 */
                MovingView(Tab, 10, 10, -1, -1);
                MovingView(ChangeTab, 20, Tab.Height + 17, -1, -1);
                MovingView(Shift, 30, ChangeTab.Height + Tab.Height + 27, -1, -1);
                MovingView(BackLabel, width - SubKeySize.Width - 10, 10, -1, -1);
                MovingView(Enter, width - SubKeySize.Width - 20, SubKeySize.Height + 17, -1, -1);

                /* 座標計算 */
                double x = width - Enter.Width - UpLabel.Width - 27;
                double y = BackLabel.Height + Enter.Height + 24;

                /* 矢印キーの配置 */
                MovingView(UpLabel, x, y, -1, -1);
                MovingView(LeftLabel, x - LeftLabel.Width - 7, y + UpLabel.Height + 7, -1, -1);
                MovingView(DownLabel, x, y + UpLabel.Height + 7, -1, -1);
                MovingView(RightLabel, x + DownLabel.Width + 7, y + UpLabel.Height + 7, -1, -1);
            }
            else
            {
                /* パネルを隠す */
                MovingView(PCKeyPane, -1, -1, -1, 0);
            }

            Showing = false;

            IsPCKeyPaneShow = !IsPCKeyPaneShow;
        }

        private void ShowShortCutPane()
        {
            Showing = true;

            /* パネルの表示非表示ごとに処理 */
            if (!IsShortCutPaneShow)
            {
                /*
                Rectangle rc = FloatingPane.Bounds;
                rc.X -= ShortCutPane.Width;
                SlideFloatings(rc);
                */

                /* フローティングボタンもどきの移動 */
                MovingView(FloatingPane, FloatingPane.X - ShortCutPane.Width, -1, -1, -1);

                /* パネルを出す 
                rc = ShortCutPane.Bounds;
                rc.X = MainLayout.Width - ShortCutPane.Width;
                await ShortCutPane.LayoutTo(rc, 120);
                */

                /* ショートカットパネルを出す */
                MovingView(ShortCutPane, MainLayout.Width - ShortCutPane.Width, -1, -1, -1);

                /*
                rc = ArrowPane.Bounds;
                rc.Y += Width / 7;
                ArrowPane.LayoutTo(rc, 100);
                */

                /* PCキーパネルのサイズを変更 */
                MovingView(PCKeyPane, -1, PCKeyPane.Y + Width / 7, PCKeyPane.Width - ShortCutPane.Width, -1);

                /* PCキーパネルが表示済みの場合はキーの配置を最適化 */
                if (IsPCKeyPaneShow)
                {
                    double width = Width - ShortCutPane.Width;

                    MovingView(Tab, 10, 10, -1, -1);
                    MovingView(ChangeTab, 20, Tab.Height + 17, -1, -1);
                    MovingView(Shift, 30, ChangeTab.Height + Tab.Height + 27, -1, -1);
                    MovingView(BackLabel, width - SubKeySize.Width - 10, 10, -1, -1);
                    MovingView(Enter, width - SubKeySize.Width - 20, SubKeySize.Height + 17, -1, -1);

                    double x = width - Enter.Width - UpLabel.Width - 27;
                    double y = BackLabel.Height + Enter.Height + 24;

                    MovingView(UpLabel, x, y, -1, -1);
                    MovingView(LeftLabel, x - LeftLabel.Width - 7, y + UpLabel.Height + 7, -1, -1);
                    MovingView(DownLabel, x, y + UpLabel.Height + 7, -1, -1);
                    MovingView(RightLabel, x + DownLabel.Width + 7, y + UpLabel.Height + 7, -1, -1);
                }

                /* エディターのサイズを変える 
                rc = SendEditor.Bounds;
                rc.Width -= ShortCutPane.Width;
                rc.Height += Width / 7;
                await SendEditor.LayoutTo(rc, 100);
                */

                /* エディターのサイズを変える */
                MovingView(SendEditor, -1, -1, SendEditor.Width - ShortCutPane.Width, SendEditor.Height + Width / 7);
            }
            else
            {
                /*
                Rectangle rc = FloatingPane.Bounds;
                rc.X += ShortCutPane.Width;
                SlideFloatings(rc);
                */

                /* フローティングボタンもどきを移動 */
                MovingView(FloatingPane, FloatingPane.X + ShortCutPane.Width, -1, -1, -1);

                /* パネルを隠す 
                rc = ShortCutPane.Bounds;
                rc.X = MainLayout.Width;
                await ShortCutPane.LayoutTo(rc, 120);
                */

                /* ショートカットパネルを隠す */
                MovingView(ShortCutPane, Width, -1, -1, -1);

                /*
                rc = ArrowPane.Bounds;
                rc.Y -= Width / 7;
                ArrowPane.LayoutTo(rc, 100);
                */

                /* PCキーパネルの移動とサイズ変更 */
                MovingView(PCKeyPane, -1, PCKeyPane.Y - Width / 7, Width, -1);

                /* PCキーパネルが表示済みの時は配置の最適化をする */
                if (IsPCKeyPaneShow)
                {
                    double width = Width;

                    MovingView(Tab, 10, 10, -1, -1);
                    MovingView(ChangeTab, 20, Tab.Height + 17, -1, -1);
                    MovingView(Shift, 30, ChangeTab.Height + Tab.Height + 27, -1, -1);
                    MovingView(BackLabel, width - SubKeySize.Width - 10, 10, -1, -1);
                    MovingView(Enter, width - SubKeySize.Width - 20, SubKeySize.Height + 17, -1, -1);

                    double x = width - Enter.Width - UpLabel.Width - 27;
                    double y = BackLabel.Height + Enter.Height + 24;

                    MovingView(UpLabel, x, y, -1, -1);
                    MovingView(LeftLabel, x - LeftLabel.Width - 7, y + UpLabel.Height + 7, -1, -1);
                    MovingView(DownLabel, x, y + UpLabel.Height + 7, -1, -1);
                    MovingView(RightLabel, x + DownLabel.Width + 7, y + UpLabel.Height + 7, -1, -1);
                }

                /* エディターのサイズを元に戻す 
                rc = SendEditor.Bounds;
                rc.Width += ShortCutPane.Width;
                rc.Height -= Width / 7;
                await SendEditor.LayoutTo(rc, 100);
                */

                /* エディターのサイズを元に戻す */
                MovingView(SendEditor, -1, -1, SendEditor.Width + ShortCutPane.Width, SendEditor.Height - Width / 7);
            }

            IsShortCutPaneShow = !IsShortCutPaneShow;

            Showing = false;
        }

        /*
        private async void SlideFloatings(Rectangle rc)
        {
            await FloatingPane.LayoutTo(rc, 120);
        }
        */

        /* データバインディング用構造体 */
        class LabelSize
        {
            public double Width { get; set; }

            public double Height { get; set; }

            public LabelSize(double w, double h)
            {
                Width = w;
                Height = h;
            }
        }

        class ShortCutListViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ShortCutListItem> ShortCutItems { get; set; }

            public ShortCutListViewModel()
            {
                ShortCutItems = new ObservableCollection<ShortCutListItem>(new[]
                {
                    new ShortCutListItem { Title = "コピー", Send = "<COP>" },
                    new ShortCutListItem { Title = "カット", Send = "<CUT>" },
                    new ShortCutListItem { Title = "ペースト", Send = "<PAS>" },
                    new ShortCutListItem { Title = "全選択", Send = "<ALL>" },
                    new ShortCutListItem { Title = "戻る", Send = "<BEF>" },
                    new ShortCutListItem { Title = "次へ進む", Send = "<AFT>" },
                    new ShortCutListItem { Title = "検索", Send = "<SEA>" },
                    new ShortCutListItem { Title = "開く", Send = "<OPN>" },
                    new ShortCutListItem { Title = "保存(名)", Send = "<NSA>" },
                    new ShortCutListItem { Title = "保存(上)", Send = "<SAV>" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}