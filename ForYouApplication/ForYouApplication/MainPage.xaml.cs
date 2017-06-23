using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

/* using ForYouApplication.Droid; */

namespace ForYouApplication
{
	public partial class MainPage : ContentPage
	{
        private readonly string ADDRESSREGULAREXPERSSION = "";

        private string address;

        private ManualResetEvent mre = new ManualResetEvent(false);

        public MainPage()
		{
            InitializeComponent();
		}

        /* ボタン押下に呼び出される*/
        private void OnClick(Object sender, EventArgs args)
        {
            /*テスト用*/
            if (sender.Equals(this.Button1))
            {
                this.Label1.Text = "座マリウス";


                mre.Set();


                /*
                Debug.WriteLine("---------------------アドレス---------------------");
                IPAddress[] adrList = await Dns.GetHostAddressesAsync(Dns.GetHostName());
                foreach (IPAddress address in adrList)
                {
                    Debug.WriteLine(address);
                }
                Debug.WriteLine("---------------------アドレス---------------------");
                */
            }
            else if (sender.Equals(this.Button2))
            {
                var hostAddress = this.Address.Text;
                this.Label1.Text = hostAddress;

                /* デバイス分け */
                if (Device.RuntimePlatform == Device.Android)
                {
                    
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {

                }
                
                Debug.WriteLine("--------------------Android端末用--------------------");
                try
                {
                    /* AsynchronousClient.StartClient(); */

                    /* Socket client = new ConnectTest().Connection(hostAddress); */

                    /*
                    Client client = new Client();
                    client.Connection(hostAddress);
                    */

                    AsyncTcpClient client = new AsyncTcpClient();
                    if (client.Connection(hostAddress))
                    {
                        new AsyncReceiver(client).ReceiveTask();
                        Navigation.PushAsync(new InputForm(client), true);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("--------------------例外--------------------");
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        public void OnClick2(Object sender, EventArgs args)
        {
            if (sender.Equals(this.Button3))
            {
                mre.WaitOne(5000);
                Debug.WriteLine("-----------通過-----------");
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
                    address = result.Text;
                });
            };
        }
    }
}
