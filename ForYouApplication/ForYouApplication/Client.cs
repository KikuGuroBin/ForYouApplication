using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class Client
    {
        private const int PORT = 55555;

        private TcpClient client;

        public void Connection(String hostAddress)
        {
            Debug.WriteLine("接続開始");

            //TcpClientを作成し、サーバーと接続する
            client = new TcpClient(hostAddress, PORT);

            Debug.WriteLine("接続");
        }

        public void Send(string command, string data)
        {
            StringBuilder sb = new StringBuilder(command);
            sb.Append(data);

            string sendData = sb.ToString();
            
            //NetworkStreamを取得する
            NetworkStream ns = client.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            //サーバーにデータを送信する
            //文字列をByte型配列に変換
            byte[] sendBytes = Encoding.UTF8.GetBytes(sendData + '\n');
            //データを送信する
            ns.Write(sendBytes, 0, sendBytes.Length);
            ns.Close();
            Debug.WriteLine(sendData);
        }

        public string Receive()
        {
            //NetworkStreamを取得する
            NetworkStream ns = client.GetStream();

            //サーバーから送られたデータを受信する
            MemoryStream ms = new MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                
                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける
            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
            //受信したデータを文字列に変換
            string resData = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);

            ns.Close();
            ms.Close();

            //末尾の\nを削除
            resData = resData.TrimEnd('\n');

            return resData;
        }

        public void DisConnect()
        {
            client.Close();
        }
    }
}
