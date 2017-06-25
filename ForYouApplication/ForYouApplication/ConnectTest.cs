using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ForYouApplication
{
    public class ConnectTest
    {
        /* ポート番号の定数 */
        private readonly Int32 PORT = 55555;

        /* メインスレッドのマニュアル制御用 */
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        /* ホストとの接続確立後のSocket格納用 */
        private Socket client;
        /* ホストからの受信データ格納用 */
        private string response;

        /* ホストと接続するためのSocket生成 */
        public Socket Connection(string address)
        {
            IPAddress HostAddress = null;
            try
            {
                /* ホストのIPAddressを生成 */
                HostAddress = IPAddress.Parse(address);
            }
            catch (FormatException e)
            {
                /* アドレスの書式が不正 */
                Debug.WriteLine(e.StackTrace);
            }
            catch (ArgumentException e)
            {
                /* nullを渡された */
                Debug.WriteLine(e.StackTrace);
            }

            /* Socket生成 */
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Debug.WriteLine("deg : Socket生成");

            /* ホストとの接続 */
            client.BeginConnect(HostAddress, PORT, new AsyncCallback(ConnectCallback), client);
            /* スレッド待機 */
            connectDone.WaitOne();

            Debug.WriteLine("deg : スレッド再開");

            return client;
        }
 
        /* ホストとの接続確立後の処理 */
        public void ConnectCallback(IAsyncResult ar)
        {
            Debug.WriteLine("deg : Callback開始");

            /* IAsyncResultからSocketを取り出す */
            Socket socket = (Socket)ar.AsyncState;
            /* ホストからの最初のメッセージを格納 */
            //response = new Communication().ConnectWait(socket);

            /* デバッグ */
            Debug.WriteLine(response);

            /* スレッド再開 */
            connectDone.Set();
        }
    }
}