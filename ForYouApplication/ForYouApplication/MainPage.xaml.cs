using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ForYouApplication
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            InitializeComponent();
		}

        /*2017/5/31 追加
           ボタン押下に呼び出される*/
        private void OnClick(Object sender, EventArgs args)
        {
            /*テスト用*/
            if (sender.Equals(this.Button1))
            {
                this.Label1.Text = "座マリウス";
            }
        }

        /*2017/6/1 追加
         * QRコードスキャンボタン用*/
        async void ScanButtonClick(object sender, EventArgs s)
        {
            // スキャナページの設定
            var scanPage = new ZXingScannerPage()
            {
                DefaultOverlayTopText = "バーコードを読み取ります",
                DefaultOverlayBottomText = "",
            };
            // スキャナページを表示
            await Navigation.PushAsync(scanPage);

            // データが取れると発火
            scanPage.OnScanResult += (result) =>
            {
                // スキャン停止
                scanPage.IsScanning = false;

                // PopAsyncで元のページに戻り、結果をダイアログで表示
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("スキャン完了", result.Text, "OK");
                });
            };
        }
    }
}
