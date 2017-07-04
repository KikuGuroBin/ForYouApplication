using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendFormPage : MasterDetailPage
    {
        /* リモートホストとの通信用クライアント */
        private AsyncTcpClient Client;

        /* Editor.Textの一時保存用 */
        private string SaveText;

        /* 送信した全テキストの格納用 */
        private Queue<string> SendLog = new Queue<string>();

        /* デバッグ用コンストラクタ */
        public SendFormPage()
        {
            InitializeComponent();

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);
            MasterPage.ListView.ItemSelected += ListViewItemSelected;
            DetailPage.Editor.TextChanged += EditorTextChanged;
            DetailPage.Editor.Completed   += EditorCompleted;
        }

        public SendFormPage(AsyncTcpClient Client)
        {
            InitializeComponent();

            this.Client = Client;

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);

            /* イベントハンドラー設定 */
            DetailPage.Editor.TextChanged    += EditorTextChanged;
            MasterPage.ListView.ItemSelected += ListViewItemSelected;
        }

        /* ボタンが押されたときのイベント */
        private async void OnClick(object sender, EventArgs args)
        {
            /* アラート表示 */
            bool result = await DisplayAlert("確認", "本当に切断しますか?", "OK", "Cancel");

            /* OKを選択したとき */
            if (result)
            {
                /* リモートホストに切断要求をして、画面遷移 */
                /* Client.Send("<END>"); */
                await Navigation.PopAsync(true);
            }
        }
        
        /* Master内のアイテムが選択されたときのイベント */
        private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /* 選択されたメニューアイテム取得 */
            var item = e.SelectedItem as SendFormMenuItem;
            /* アイテムのフィールドIDを取得 */
            int id   = item
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
                /* Client.Send("SHORTCUT", result); */
            }
            /* 送信ログを選択した場合 */
            else if (id == 3)
            {
                /* 蓄積した送信ログを配列に書き出す */
                string[] log = SendLog.ToArray();

                /* アラートシート表示 */
                string result = await DisplayActionSheet("送信ログ", "", "", log);

                /* 
                 * DependencyServiceに設定した、
                 * 各プラットフォームで作成したIClipBoardを実装したクラスを呼び出し、
                 * クラスのメソッドを使って選択したログをクリップボードに保存
                 */
                DependencyService.Get<IClipBoard>().SaveClipBoard(result);
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
                    /* Client.Send("<ENDCONNECTION>", "end"); */
                    await Navigation.PopAsync(true);
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
            /* Editorの文字を格納用 */
            string workText = DetailPage.Editor.Text;
            /* 加工後の送信文字列格納用 */
            string work     = "";
            /* 文字数格納用 */
            int textLength  = workText.Length;

            if (workText.IndexOf("\n") > -1)
            {
                if (workText == "")
                {
                  /*  work = "\n"; */
                }
                else
                {
                    /*
                    SaveText = "";
                    SendLog.Enqueue(workText);
                */
                    }
            }
            else
            {
                work = SaveText == null ? workText : SendContentDeicsion(SaveText, workText).result;
            }

            /* リモートホストへ送信 */
            /* Client.Send(work); */

            System.Diagnostics.Debug.WriteLine(work);

            /* 現在のEditorのテキストの内容を保存 */
            SaveText = workText;
        }

        /* Editorに入力された文字が確定されたときのイベント */
        private void EditorCompleted(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("deg : Completed-----------------");
        }

        /* バックボタン押下時のイベント */
        protected override bool OnBackButtonPressed()
        {
            /* 何もせずにtrueを返すことでバックキーを無効化 */
            return true;
        }

        /* リモートホストに送信する文字とタグの判定 */
        private (int previous, int behind, string result) SendContentDeicsion(string before, string after)
        {
            /* 各文字列を一文字ずつに分割して配列に格納 */
            char[] befArr = before.ToCharArray();
            char[] aftArr = after.ToCharArray();
            /* 各配列の要素数(文字数)格納 */
            int befLen = befArr.Length;
            int aftLen = aftArr.Length;

            /* 二つの配列のうち要素の少ないほうの値を下記のループ文の継続条件に使う */
            int lenMin = Math.Min(befLen, aftLen);
            
            /* ループ用インデックス */
            int preIndex = 0;
            /* 各配列の先頭から見ていき、最初に一致しなくなる場所を探す */
            for (; preIndex < lenMin; preIndex++)
            {
                char befChar = befArr[preIndex];
                char aftChar = aftArr[preIndex];

                if (!befChar.Equals(aftChar))
                {
                    break;
                }
            }
            
            /* 一致しない文字がなかった場合 */
            if (preIndex == lenMin)
            {
                /* 変更後の文字数のほうが少なかった場合、バックスペースをされたと判定する */
                if (lenMin == aftLen)
                {
                    return (1, 0, TagConstants.DELETE.GetConstants());
                }
                /* 
                 * 変更後の文字数のほうが多く、文字数の差が1だった場合、
                 * 通常のキー入力がされたと判定する 
                 */
                else if (lenMin == befLen && aftLen - befLen == 1)
                {
                    return (0, 0, aftArr[lenMin].ToString());
                }
            }

            /* ループ用インデックス */
            int behiIndex = Math.Max(befLen, aftLen);

            /* 文字数の差を求める */
            (int be, int af) diff = befLen == aftLen ?
                                    (0, 0) : aftLen > befLen ?
                                            (aftLen - befLen, 0) : (0, befLen - aftLen);

            /* 配列を最後尾から見ていき、最初に一致しなかった場所を探す */
            for (int di = Math.Max(diff.be, diff.af); behiIndex - di > -1; behiIndex--)
            {
                char befChar = befArr[behiIndex - diff.be];
                char aftChar = aftArr[behiIndex - diff.af];

                if (!befChar.Equals(aftChar))
                {
                    break;
                }
            }

            return (preIndex, behiIndex, "");
        }

        private void Conv(string a, string b)
        {
            var work = a.Substring(0, 1);
            var save = "";

            if (b.IndexOf(work) > -1)
            {

            }

            for (int i = 2; i < 10; i++)
            {
                work = a.Substring(0, i);

                if (b.IndexOf(work) > -1)
                {
                    save = work;
                }
            }
        }
    }
}