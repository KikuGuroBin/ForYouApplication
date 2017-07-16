using System.Text;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class AsyncTcpClient
    {
        private const int BYTESIZE = 1024;
        private const int TIMEOUT = 30000;

        private TcpClient Client;
       
        public AsyncTcpClient(TcpClient Client)
        {
            this.Client = Client;
        }

        /* 送信用 */
        public async void Send(string data)
        {
            /* NetworkStreamを取得する */
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.WriteTimeout  = TIMEOUT;

            /* ホストに送信する文字列をByte型配列に変換 */
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);

            /* データを送信する */
            await ns.WriteAsync(sendBytes, 0, sendBytes.Length);
        }

        /* 
         * 受信用
         * 接続開始から非同期で動かす
         * 現段階では実質リモートホストからの切断要求受付用
         */
        public async void Receive()
        {
            /* NetworkStreamを取得する */
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.ReadTimeout   = TIMEOUT;
            
            /* 受信データ格納用 */
            byte[] buffer    = new byte[BYTESIZE];
            
            /* データを受信するまでここでスレッドをブロックする */
            await ns.ReadAsync(buffer, 0, buffer.Length);

            /* 切断 */
            Client.Close();
        }

        /* 切断 */
        public void Disconnect()
        {
            Client.Close();
        }
    }
}