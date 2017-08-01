using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Net.Sockets;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ForYouApplication
{
    public class Cr_scam:ContentPage
    {
        async public void Cr_scan()
        {
            /* スキャナページの設定 */
            ZXingScannerPage scanPage = new ZXingScannerPage()
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

                    /* ホストアドレス取得 */
                    string address = result.Text;

                    try
                    {
                        TcpClient client = new TcpClient();

                        /* 接続 */
                        await client.ConnectAsync(address, 55555);

                        /* 送信文字入力画面へ画面遷移 */
                        await Navigation.PushAsync(new SendFormPage(client), false);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }
                });
            };

        }
        
    }
}
