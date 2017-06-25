using System;
using System.Diagnostics;
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

        /* ボタン押下に呼び出される */
        private void OnClick(Object sender, EventArgs args)
        {
            /* テスト用 */
            if (sender.Equals(this.Button1))
            {
                this.Label1.Text = "座マリウス";
            }
            else if (sender.Equals(this.Button2))
            {
                string hostAddress = this.Address.Text;
                
                try
                {
                    /* AsyncTcpClient生成 */
                    AsyncTcpClient client = new AsyncTcpClient();

                    /* リモートホストと接続 */
                    if (client.Connection(hostAddress))
                    {
                        /* 受信待ち開始 */
                        client.Receive();

                        /* デバイス分け */
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            Debug.WriteLine("--------------------Android端末用--------------------");

                            /* 送信文字入力画面へ画面遷移 */
                            Navigation.PushAsync(new InputForm(client), true);
                        }
                        else if (Device.RuntimePlatform == Device.iOS)
                        {
                            Debug.WriteLine("--------------------iOS端末用--------------------");

                            /* 送信文字入力画面へ画面遷移 */
                            Navigation.PushAsync(new InputForm(client), true);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("--------------------例外--------------------");
                    Debug.WriteLine(e.StackTrace);
                }
            }
            else if (sender.Equals(Sock))
            {
                string hostAddress = this.Address.Text;

                System.Net.Sockets.Socket client = new ConnectTest().Connection(hostAddress);
            }
            
        }

        /* QRコードスキャンボタン用*/
        async void ScanButtonClick(object sender, EventArgs s)
        {
            /* スキャナページの設定 */
            var scanPage = new ZXingScannerPage()
            {
                DefaultOverlayTopText = "バーコードを読み取ります",
                DefaultOverlayBottomText = "",
            };

            /*スキャナページを表示*/
            await Navigation.PushAsync(scanPage);

            /*データが取れると発火*/
            scanPage.OnScanResult += (result) =>
            {
                /*スキャン停止*/
                scanPage.IsScanning = false;

                /*PopAsyncで元のページに戻り、結果をダイアログで表示*/
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("スキャン完了", result.Text, "OK");
                });
            };
        }
    }
}