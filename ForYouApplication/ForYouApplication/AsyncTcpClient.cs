using System;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class AsyncTcpClient
    {
        private const int PORT = 55555;
        private const int BYTESIZE = 256;
        private const int TIMEOUT = 30000;
        private const string DEFAULTRECEIVEDATA = "NONE";
        private const string ENDCONNECTION = "<ENDCONN>";

        private TcpClient Client;

        /* 接続用 */
        public bool Connection(String hostAddress)
        {
            bool flag = true;

            try
            {
                /* TcpClientを作成し、リモートホストと接続する */
                Client = new TcpClient(hostAddress, PORT);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                flag = false;
            }

            return flag;
        }

        /* 送信用 */
        public void Send(params string[] data)
        {
            string sendData = "";

            if (data.Length <= 1)
            {
                sendData = data[0];
            }
            else
            {
                /* タグとデータの結合 */
                StringBuilder sb = new StringBuilder(data[0]);
                sb.Append(data[1]);

                /* string型に変換 */
                sendData = sb.ToString();
            }

            /* NetworkStreamを取得する */
            NetworkStream ns = Client.GetStream();

            Debug.WriteLine(ns.WriteTimeout);

            /* タイムアウト設定(30秒) */
            ns.WriteTimeout  = TIMEOUT;

            /* ホストに送信する文字列をByte型配列に変換 */
            byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);

            /* データを送信する */
            ns.Write(sendBytes, 0, sendBytes.Length);
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
            
            /* データを受信するまでここでメインスレッドをブロックする */
            await ns.ReadAsync(buffer, 0, buffer.Length);

            /* デバッグ用 */
            Debug.WriteLine("--------------受信--------------");

            /* 切断 */
            Client.Close();


            /* ------------------以下メモ------------------ */


            /* 読み取り不可になるまで受信を続ける 
                while (ns.DataAvailable)
                {
                
                }
            */


            /*
                await Task.Run(() =>
                {
                    /* NetworkStreamを取得する
                    NetworkStream ns = Client.GetStream();

                    /* 受信不可になるまでここで止める
                    while (Client.Connected) {}

                    Debug.WriteLine("--------------切断--------------");
                });
            */


            /* ↓MemoryStreamのでの受信

                /* リモートホストから送られたデータ格納用 
                MemoryStream ms = new MemoryStream();
                string data = DEFAULTRECEIVEDATA;
                byte[] buffer = new byte[BYTESIZE];
                int bufferSize = 0;

                /* 読み取り不可になるまで受信を続ける 
                while (ns.DataAvailable)
                {
                    //データの一部を受信する
                    bufferSize = await ns.ReadAsync(buffer, 0, buffer.Length);

                    //受信したデータを蓄積する
                    Encoding.UTF8.GetString(buffer, 0, bufferSize);
                }

                /* 受信したデータをstring型に変換 
                data = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);

                /* ストリームを閉じる 
                ms.Close();
            */
        }

        /* 切断 */
        public void Disconnect()
        {
            Client.Close();
        }
    }
}