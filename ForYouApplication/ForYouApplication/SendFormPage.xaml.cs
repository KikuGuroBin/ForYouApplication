using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
            DetailPage.Editor.TextChanged += EditorTextChanged;
        }

        public SendFormPage(TcpClient client)
        {
            InitializeComponent();
            
            Client = new AsyncTcpClient(client);

            /* 受信待ち開始 */
            Client.Receive();

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);

            /* イベントハンドラー設定 */
            DetailPage.Editor.TextChanged += EditorTextChanged;
            MasterPage.ListView.ItemSelected += ListViewItemSelected;
        }
        
        /* Master内のアイテムが選択されたときのイベント */
        private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /* 選択されたメニューアイテム取得 */
            var item = e.SelectedItem as SendFormMenuItem;
            /* アイテムのフィールドIDを取得 */
            int id = item
                ?. Id
                ?? -1;
            
            if(id == -1)
            {
                return;
            }
            
            /* ショートカットキーを選択した場合 */
            if (id == 4)
            {
                /* アラートシート表示 */
                string result = await DisplayActionSheet("選択してください", "", "", "コピー", "ペースト", "カット");

                /* リモートホストに送信 */
                Client.Send("SHORTCUT");
            }
            /* 送信ログを選択した場合 */
            else if (id == 3)
            {
                /* 蓄積した送信ログを配列に書き出す */
                string[] log = SendLog.ToArray();

                /* アラートシート表示 */
                string result = await DisplayActionSheet("送信ログ", "", "", log);

                if (result == "")
                {
                    /* 
                     * DependencyServiceに設定した、
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
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                Detail = new NavigationPage(new ITrackPad());
                IsPresented = false;
            }

            MasterPage.ListView.SelectedItem = null;
        }

        /* Editorのテキストが変更(入力、変換、削除)されたときのイベント */
        private void EditorTextChanged(object sender, EventArgs args)
        {
            /* TextChange中からのTextChange発生を阻止 */
            if (EditorFlag)
            {
                return;
            }

            /* テキスト送信とEditor.Text操作 */
            SendText(DetailPage.Editor.Text);
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
            await Task.Run(() =>
            {
                /* テキストチェンジ開始 */
                EditorFlag = true;
                
                /* 加工後の文字列とEditorを操作するための数値を取得 */
                (int index, int length, string send) process = SaveText == null ? 
                                                                (-1, -1, text) : TextProcess.SendContentDeicsion(SaveText, text);

                /* リモートホストへ送信 
                Client.Send(process.send);
                */

                if (process.index > -1)
                {
                    /* Editor内の文字列操作 */
                    string save = text.Substring(0, process.index + 1);
                    text.Remove(0, process.index + 1);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DetailPage.Editor.Text = text;
                    });

                    /* ログに格納する */
                    SendLog.Enqueue(save);
                }

                /* クライアントが改行した場合 
                if (workText.IndexOf("\n") > -1)
                {
                    /* Editor内に改行以外の文字があった場合 
                    if (workText.Length > 1)
                    {
                        int index = process.index;

                        if (index > -1)
                        {
                            /* ログに残す文字抽出とEditor 
                            string save = workText.Remove(index, process.length);

                            /* Editorの文字操作 
                            DetailPage.Editor.Text = workText;

                            /* ログに格納する 
                            SendLog.Enqueue(save);
                        }
                    }
                    else
                    {
                        /* Editorを空白にする 
                        DetailPage.Editor.Text = "";
                    }
                }
                */

                /* 現在のEditorのテキストの内容を保存 */
                SaveText = text;
                
                /* テキストチェンジ終了 */
                EditorFlag = false;
            });
        }
    }
}