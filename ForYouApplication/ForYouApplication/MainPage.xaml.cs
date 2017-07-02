using System;
using System.Diagnostics;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ForYouApplication
{
	public partial class MainPage : ContentPage
	{

        private const string IPADDRESSREGULAREXPRESSION = "[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}";
        
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
                string[] a = { "1.1.1.1", "10.2.1.10", "255.255.255.255", "192.168.2.101" };
                foreach(var i in a)
                {
                    //Debug.WriteLine(i.(IPADDRESSREGULAREXPRESSION));
                }
            }
            else if (sender.Equals(Intent))
            {
                Navigation.PushAsync(new MasterDetailPage1(), true);
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

            /* スキャナページを表示 */
            await Navigation.PushAsync(scanPage);

            /* データが取れると発火 */
            scanPage.OnScanResult += (result) =>
            {
                /* スキャン停止 */
                scanPage.IsScanning = false;
                
                /* 
                 * PopAsyncで元のページに戻り、取得したIPアドレスをもとに
                 * AsyncTcpClient生成
                 */
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    string address = result.Text;

                    /* AsyncTcpClient生成 */
                    AsyncTcpClient client = new AsyncTcpClient();

                    /* リモートホストと接続 */
                    if (client.Connection(address))
                    {
                        /* 受信待ち開始 */
                        client.Receive();

                        try
                        {
                            /* デバイス分け */
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                Debug.WriteLine("--------------------Android端末用--------------------");

                                /* 送信文字入力画面へ画面遷移 */
                                await Navigation.PushAsync(new MasterDetailPage1(client), true);
                            }
                            else if (Device.RuntimePlatform == Device.iOS)
                            {
                                Debug.WriteLine("--------------------iOS端末用--------------------");

                                /* 送信文字入力画面へ画面遷移 */
                                await Navigation.PushAsync(new InputForm(client), true);
                            }
                        }
                        catch(Exception e)
                        {
                            Debug.WriteLine(e.StackTrace);
                        }
                    }
                });
            };
        }
    }
}