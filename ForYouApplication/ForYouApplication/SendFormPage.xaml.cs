using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendFormPage : MasterDetailPage
    {
        /* リモートホストとの通信用クライアント */
        private AsyncTcpClient Client;

        /* Editorの一時保存用 */
        private string SaveText;

        /* 送信した全テキストの格納用 */
        private Queue<string> SendLog = new Queue<string>();

        /* Editor.TextChange制御用 */
        private bool EditorFlag;

        /* デバッグ用コンストラクタ */
        public SendFormPage()
        {
            InitializeComponent();

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);

            DetailPage.Editor.TextChanged += EditorTextChanged;
        }

        public SendFormPage(TcpClient client)
        {
            InitializeComponent();

            Client = new AsyncTcpClient(client);

            /* リモートホスト名取得 */
            SetHostName();

            /* 受信待ち開始 */
            Client.Receive();

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);

            /* イベントハンドラー設定 */
            DetailPage.Editor.TextChanged += EditorTextChanged;

            DetailPage.ShortCutList.ItemSelected += ShortCutListItemSelected;

            MasterPage.ListView.ItemSelected += ListViewItemSelected;
            
            TapGestureRecognizer ges = new TapGestureRecognizer();
            ges.Tapped += (s, e) => OnLabelClicked(s, e);
            DetailPage.Uplabel1.GestureRecognizers.Add(ges);
            DetailPage.Backlabel1.GestureRecognizers.Add(ges);
            DetailPage.Leftlabel1.GestureRecognizers.Add(ges);
            DetailPage.Rightlabel1.GestureRecognizers.Add(ges);
            DetailPage.Downlabel1.GestureRecognizers.Add(ges);
            DetailPage.Shift.GestureRecognizers.Add(ges);
            DetailPage.Tab.GestureRecognizers.Add(ges);
            DetailPage.ChangeTab.GestureRecognizers.Add(ges);
            DetailPage.Editor.GestureRecognizers.Add(ges);


        }

        /* 画面がアンロード(アプリ終了時)に呼ばれる */
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            /* リモートホストとの接続を切る */
            Client.Disconnect();
        }

        /* リモートホストの名前を取得する */
        private async void SetHostName()
        {

            StringBuilder name = new StringBuilder(await Client.GetHostName());
            name.Append(" と接続中...");

            Device.BeginInvokeOnMainThread(() =>
            {
                /* Master部分のListView.Headerにリモートホスト名を設定 */
                MasterPage.Header.Text = name.ToString();
            });
        }

        /* ラベルタップ用イベント */
        private void OnLabelClicked(object sender, EventArgs e)
        {
            
            if (sender.Equals(DetailPage.Backlabel1))
            {
                if (!DetailPage.ShiftFlag)
                {
                    Client.Send("<BAC>");
                }
            }
            else if (sender.Equals(DetailPage.Uplabel1))
            {
                Client.Send("<UPP>");
            }
            else if (sender.Equals(DetailPage.Rightlabel1))
            {
                Client.Send("<RIG>");
            }
            else if (sender.Equals(DetailPage.Downlabel1))
            {
                Client.Send("<DOW>");
            }
            else if (sender.Equals(DetailPage.Leftlabel1))
            {
                Client.Send("<LEF>");
            }
            else if (sender.Equals(DetailPage.Shift))
            {
                Client.Send("<SHI>");
            }
            else if (sender.Equals(DetailPage.Tab))
            {
                Client.Send("<TAB>");
            }
            else if (sender.Equals(DetailPage.Tab))
            {
                Client.Send("<ENT>");
            }
            else if (sender.Equals(DetailPage.ChangeTab))
            {
                if (!DetailPage.ShiftFlag)
                {
                    Client.Send("<CTA>");
                }
            }
        }

        /* ショートカットリストのアイテムが選択されたときのイベント */
        private void ShortCutListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
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
            
            DetailPage.ShortCutList.SelectedItem = null;
        }

        /* Master内のアイテムが選択されたときのイベント */
        private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /* 選択されたリストアイテム取得 */
            SendFormMenuItem item = e.SelectedItem as SendFormMenuItem;

            /* アイテムのフィールドIDを取得 */
            int id = item?.Id ?? -1;

            if (id == -1)
            {
                return;
            }
            
            /* 送信ログを選択した場合 */
            if (id == 3)
            {
                /* 蓄積した送信ログを配列に書き出す */
                string[] log = SendLog.ToArray();

                /* アラートシート表示 */
                string result = await DisplayActionSheet("送信ログ", "", "", log);

                if (result != "")
                {
                    /* 
                     * Dependencyに設定した、
                     * 各プラットフォームで作成したIClipBoardを実装したクラスを呼び出し、
                     * クラスのメソッドを使って選択したログをクリップボードに保存
                     */
                    DependencyService.Get<IClipBoard>().SaveClipBoard(result);
                }
            }
            /* テンプレートを選択した場合 */
            else if (id == 2)
            {

            }
            /* トラックパッドを選択した場合 */
            else if (id == 1)
            {
                await Navigation.PushAsync(new ITrackPad());
            }
            /* 切断を選択した場合*/
            else if (id == 0)
            {
                /* アラート表示 */
                bool result = await DisplayAlert("確認", "本当に切断しますか?", "OK", "Cancel");

                /* OKを選択したとき */
                if (result)
                {
                    /* リモートホストに切断要求をして、画面遷移 */
                    Client.Send(TagConstants.END.GetConstants());
                    await Navigation.PopAsync();
                }
            }
            else
            {
                /* Detailの画面切り替え */
                Page page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                Detail = new NavigationPage(new ITrackPad());
                IsPresented = false;
            }

            MasterPage.ListView.SelectedItem = null;
        }

        /* Editorのテキストが変更(入力、変換、削除)されたときのイベント */
        private void EditorTextChanged(object sender, EventArgs args)
        {
            /* TextChange中からのTextChange発生を抑制 */
            if (!EditorFlag)
            {
                /* テキスト送信とEditor.Text操作 */
                SendText(DetailPage.Editor.Text);
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
                /* 加工後の文字列とEditorを操作するための数値を取得 */
                (int index, string send) process = SaveText == null || SaveText == "" ?
                                                   (-1, text) : TextProcess.SendContentDeicsion(SaveText, text);

                string send = process.send;

                /* リモートホストへ送信 */
                if (send == "\n")
                {
                    Client.Send("<ENT>");
                }
                else
                {
                    Client.Send(send);
                }

                /* クライアントが変換を行った場合 
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
                    /* 改行コード以外の文字がある場合 */
                    if (text.Length > 1)
                    {
                        /* ログに保存する */
                        string save = text.Replace("\n", "");
                        SendLog.Enqueue(save);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        /* Editor.Textを空白にする */
                        DetailPage.Editor.Text = "";
                    });
                }

                /* 現在のEditorのテキストの内容を保存 */
                SaveText = text;
            });

            /* テキストチェンジ終了 */
            EditorFlag = false;
        }
    }
}

