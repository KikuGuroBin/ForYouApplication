using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    class BinderForms
    {
        private readonly int SERVERPORT = 55555;
        private readonly int TIMEOUT = 30000;
        private readonly int BYTESIZE = 1000;

        private Socket client;

        public async void ServerBinder(string address)
        {
            IPAddress ipAddress = null;
            try
            {
                ipAddress = IPAddress.Parse(address);
            }
            catch (Exception e)
            { /* IP アドレスのフォーマットが不正または
                   null が指定された*/
                Console.WriteLine(e);
            }

            /* 接続 */
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, SERVERPORT);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (client.Connected == false)
            {
                try
                {
                    Console.WriteLine("接続中...");
                    client.Connect(remoteEP);
                    Console.WriteLine("接続完了!\n");
                }
                catch (SocketException)
                {
                    Console.WriteLine("接続失敗!!! 再接続します。\n");
                }
            }

            /* 文字列送信 */
            Console.WriteLine("文字列送信");
            string sendString = Console.ReadLine();
            byte[] sendData = Encoding.UTF8.GetBytes(sendString);
            client.Send(sendData, sendData.Length, SocketFlags.None);
            Console.Write("送信しました。\n");

            /* 文字列受信 */
            Console.WriteLine("サーバから受信中...");
            byte[] receiveData = new byte[BYTESIZE];
            int size = client.Receive(receiveData);
            string receiveString = Encoding.UTF8.GetString(receiveData, 0, size);
            Console.WriteLine("受信完了!");
            Console.WriteLine("{0}\n", receiveString);

            Disconnect();
        }

        public async void Disconnect()
        {
            /* 切断 */
            client.Disconnect(false);
            Console.WriteLine("切断");
        }
    }
}
