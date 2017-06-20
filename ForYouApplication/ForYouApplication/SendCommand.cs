using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ForYouApplication
{
    /* ホストへデータやコマンドを送信する */
    public class SendCommand
    {
        /* メインスレッドのマニュアル制御用 */
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private Socket client;

        public SendCommand(Socket client)
        {
            this.client = client;
        }

        /* ホストへのデータ送信 */
        public void Send(string com, string key)
        {
            /* コマンドとテキストの結合 */
            StringBuilder sb = new StringBuilder(com);
            sb.Append(key);

            /* データをバイトに変換 */
            byte[] buffer = Encoding.UTF8.GetBytes(key);
            /* ホストに対して非同期でデータを送信 */
            client.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), client);
        }

        /*送信確認用コールバック*/
        private void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

        }

        /* 送信確認用コールバック */
        private void ReceiveCallback(IAsyncResult ar)
        {

        }
    }
}