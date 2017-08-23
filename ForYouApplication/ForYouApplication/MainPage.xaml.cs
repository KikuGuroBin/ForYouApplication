using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ForYouApplication
{
	public partial class MainPage : ContentPage
	{
        private const string NETWORKERROR = "ネットワークエラー";
        private const string NETWORKERRORMESSAGE1 = "PCとスマートフォンが同じWi-Fiに接続されていない可能性があります。\n確認したのちもう一度お試しください。\nIPアドレス\n\tPC:";
        private const string NETWORKERRORMESSAGE2 = "\n\tスマフォ:";
        private const string OK = "OK";

        public MainPage()
		{
            InitializeComponent();
		}

        /* デバッグモード選択時 */
        private async void OnDebugTap(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SendFormPage(), false);
        }
        
        /* USBモード選択時のイベント */
        private async void OnUSBTap(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
            });
        }

        /* WiFiモード選択時のイベント */
        private async void OnQRTap(object sender, EventArgs args)
        {
            WiFiLabel.TextColor = Color.AliceBlue;

            /* スキャナページの設定 */
            ZXingScannerPage scanPage = new ZXingScannerPage();

            /* スキャナページを表示 */
            await Navigation.PushAsync(scanPage);

            /* データが取れると発火 */
            scanPage.OnScanResult += (result) =>
            {
                /* スキャン停止 */
                scanPage.IsScanning = false;

                /* PopAsyncで元のページに戻り、IPアドレスを取得し、リモートホストと接続を開始 */
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    /* ホストアドレス取得 */
                    string address = result.Text;
                    
                    TcpClient client = new TcpClient();

                    try
                    {
                        /* 接続 */
                        await client.ConnectAsync(address, 55555);

                        /* 送信文字入力画面へ画面遷移 
                        await Navigation.PushAsync(new SendFormPage(client), true);
                        */

                        Application.Current.MainPage = new SendFormPage(client);
                    }
                    catch(Exception)
                    {
                        /* 自分のIPアドレスを取得し、文字列に変換 */
                        IPAddress ipAddress = MyIPAddress.GetIPAddress();
                        string myAddress = ipAddress.ToString();

                        /* エラーメッセージ結合 */
                        StringBuilder message = new StringBuilder(NETWORKERRORMESSAGE1);
                        message.Append(address);
                        message.Append(NETWORKERRORMESSAGE2);
                        message.Append(myAddress);

                        /* アラートで出力 */
                        await DisplayAlert(NETWORKERROR, message.ToString(), OK);
                    }
                });
            };
        }
    }
}