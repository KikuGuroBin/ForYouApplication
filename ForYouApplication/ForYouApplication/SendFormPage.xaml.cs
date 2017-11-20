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

        /* デバッグ用コンストラクタ */
        public SendFormPage()
        {
            InitializeComponent();

            /* ナビゲーションバーを非表示にする */
            NavigationPage.SetHasNavigationBar(this, false);
            
            MasterPage.ListView.ItemSelected += ListViewItemSelected;
        }

        public SendFormPage(TcpClient client)
        {
           
        }

        /* ラベルタップ用イベント */
        private void OnLabelClicked(object sender, EventArgs e)
        {
          
        }

        /* ショートカットリストのアイテムが選択されたときのイベント */
        private void ShortCutListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /* 選択されたリストアイテム取得 */
            ShortCutListItem item = e.SelectedItem as ShortCutListItem;
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
            
            /* トラックパッドを選択した場合 */
            if (id == 1)
            {
                
            }
            
            else
            {
                /* Detailの画面切り替え */
                Page page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
               // Detail = new NavigationPage();
                IsPresented = false;
            }

            MasterPage.ListView.SelectedItem = null;
        }

        /* Editorのテキストが変更(入力、変換、削除)されたときのイベント */
        private void EditorTextChanged(object sender, EventArgs args)
        {

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
        }
    }
}