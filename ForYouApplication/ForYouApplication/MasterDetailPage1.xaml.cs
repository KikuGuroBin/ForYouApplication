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
    public partial class MasterDetailPage1 : MasterDetailPage
    {

        private AsyncTcpClient Client;
        private string SaveText;

        /* デバッグ用オーバーライド */
        public MasterDetailPage1()
        {
            InitializeComponent();
            
            DetailPage.Editor.TextChanged += EditorTextChanged;
            DetailPage.Editor.Completed   += EditorCompleted;
        }

        public MasterDetailPage1(AsyncTcpClient Client)
        {
            InitializeComponent();

            /* ナビゲーションバーを非表示にする（thisはPageオブジェクト）*/
            NavigationPage.SetHasNavigationBar(this, false);

            /* MasterPage.ListView.ItemSelected += ListView_ItemSelected; */
            

            DetailPage.Editor.TextChanged += EditorTextChanged;
            DetailPage.Editor.Completed   += EditorCompleted;

            
            
            DetailPage.Disconnect.Clicked += delegate
            {
                Client.Send("<ENDCONNECTION>", "end");
                Navigation.PopAsync(true);
            };
            


            this.Client = Client;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterDetailPage1MenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }

        /* Editorのテキストが変更(入力、変換、削除)されたときのイベント */
        private void EditorTextChanged(object sender, EventArgs args)
        {
            /* Editorの文字を格納 */
            string workText = DetailPage.Editor.Text;
            /* 文字数を格納 */
            int textLength  = workText.Length;
            
            if (workText.IndexOf("\n") > -1)
            {
                System.Diagnostics.Debug.WriteLine("改行");
            }
            else if (workText.IndexOf("\b") > -1)
            {
                System.Diagnostics.Debug.WriteLine("バックスペース");
            }

            string work = workText.Length > 0 ? workText.Substring(textLength - 1) : workText;

            Client.Send(work);

            System.Diagnostics.Debug.WriteLine(work);

            SaveText = workText;
        }

        /* Editorに入力された文字が確定されたときのイベント */
        private void EditorCompleted(object sender, EventArgs args)
        {
            SaveText = "";
        }

        /* バックボタン押下時のイベント */
        protected override bool OnBackButtonPressed()
        {
            /* 何もせずにtrueを返すことでバックキーを無効化 */
            return true;
        }



        private int[] SendContentDeicsion(string before, string after)
        {
            /* 
             * 戻り値
             * 要素番号0は比較した文字同士
             */
            int[] result = { 1, 2};
            /* バックスペースが押下された場合 */
            if (before.Length > after.Length)
            {
                //return before.Length - after.Length;
            }
            /* 入力または変換された場合 */
            else
            {
                /* 各文字列を一文字ずつに分割して配列に格納 */
                char[] beforeArray = before.ToCharArray();
                char[] afterArray  = after.ToCharArray();

                /* 二つの配列のうち要素の少ないほうの値を下記のループ文の継続条件に使う */
                int lenMin = Math.Min(before.Length, after.Length);
                /*  */
                int index = 0;
                for (; index < lenMin; index++)
                {
                    char beforeChar = beforeArray[index];
                    char afterChar  = afterArray[index];

                    if (!beforeChar.Equals(afterChar))
                    {
                        //return index;
                    }
                }

                if (index == lenMin)
                {
                    //return 0;
                }
            }

            return result;
        }
    }
}