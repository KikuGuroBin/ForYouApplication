using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class AsyncTcpClient
    {
        private const int PORT = 55555;
        private const int BYTESIZE = 256;
        private const int TIMEOUT = 30000;
        private const string DEFAULTRECEIVEDATA = "NONE";

        private TcpClient client;

        /* 接続 */
        public bool Connection(String hostAddress)
        {
            Debug.WriteLine("接続開始");
            bool flag = true;

            try
            {
                /* TcpClientを作成し、サーバーと接続する */
                client = new TcpClient(hostAddress, PORT);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                flag = false;
            }

            Debug.WriteLine("接続");

            return flag;
        }

        /* 送信 */
        public void Send(string command, string data)
        {
            /* タグとデータの結合 */
            StringBuilder sb = new StringBuilder(command);
            sb.Append(data);

            /* string型に変換 */
            string sendData = sb.ToString();

            /* NetworkStreamを取得する */
            NetworkStream ns = client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.WriteTimeout = TIMEOUT;

            /* ホストに送信する文字列をByte型配列に変換 */
            byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);

            /* データを送信する */
            ns.Write(sendBytes, 0, sendBytes.Length);

            ns.Close();
            Debug.WriteLine(sendData);
        }

        /* 受信 
         * ホスト側からの切断要求時にしか呼ばれない
         */
        public string Receive()
        {
            /* NetworkStreamを取得する */
            NetworkStream ns = client.GetStream();
            /* タイムアウト設定(30秒) */
            ns.ReadTimeout = TIMEOUT;

            /* サーバーから送られたデータ格納用 */
            MemoryStream ms = new MemoryStream();
            string resData  = DEFAULTRECEIVEDATA;
            byte[] resBytes = new byte[BYTESIZE];
            int resSize     = 0;

            /* まだ読み取れるデータがある場合、受信を続ける */
            while (ns.DataAvailable)
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);

                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
            }

            /* 受信したデータをstring型に変換 */
            resData = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);

            /* ストリームを閉じる */
            ms.Close();
            ns.Close();

            return resData;
        }

        /* 切断 */
        public void DisConnect()
        {
            client.Close();
        }
    }
}