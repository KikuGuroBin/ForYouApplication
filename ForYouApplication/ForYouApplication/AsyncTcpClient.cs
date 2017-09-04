using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

namespace ForYouApplication
{
    public class AsyncTcpClient
    {
        private const int BYTESIZE = 1024;
        private const int TIMEOUT = 30000;

        private TcpClient Client;
       
        public AsyncTcpClient(TcpClient client)
        {
            Client = client;
        }

        /* 
         * リモートホストの名前を取得する 
         * 接続時に一度だけ呼ばれる
         */
        public async Task<string> GetHostName()
        {
            /* NetworkStreamを取得する */
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.ReadTimeout = TIMEOUT;

            /* 受信データ格納用 */
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[BYTESIZE];

            /* データを受信するまでここでスレッドをブロックする */
            int bytes = await ns.ReadAsync(buffer, 0, buffer.Length);

            ms.Write(buffer, 0, bytes);

            string read = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);

            return read;
        }

        /* 送信用 */
        public async void Send(string data)
        {
            /* NetworkStreamを取得する */
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.WriteTimeout  = TIMEOUT;

            /* ホストに送信する文字列をByte型配列に変換 */
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            /* データを送信する */
            await ns.WriteAsync(buffer, 0, buffer.Length);
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
            ns.ReadTimeout = TIMEOUT;
            
            /* 受信データ格納用 */
            byte[] buffer = new byte[BYTESIZE];
            
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