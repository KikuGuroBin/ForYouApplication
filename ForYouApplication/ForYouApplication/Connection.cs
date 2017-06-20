using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace ForYouApplication
{
    public class Connection
    {
        /* 接続先ポート */
        private readonly int PORT = 55555;

        /* Socketクライアント */
        private TCPClient tcpClient = new TCPClient();
        private string address;

        delegate void DisconnectedDelegate(object sender, EventArgs e);
        delegate void ReceiveDelegate(object sender, string e);

        /** コンストラクタ **/
        public Connection(String address)
        {
            this.address = address;

            /* 接続OKイベント */
            tcpClient.OnConnected += new TCPClient.ConnectedEventHandler(tClient_OnConnected);
            /* 接続断イベント */
            tcpClient.OnDisconnected += new TCPClient.DisconnectedEventHandler(tClient_OnDisconnected);
            /* データ受信イベント */
            tcpClient.OnReceiveData += new TCPClient.ReceiveEventHandler(tClient_OnReceiveData);
        }

        /** 接続断イベント **/
        void tClient_OnDisconnected(object sender, EventArgs e)
        {
            /*
            if (this.InvokeRequired)
            {
                this.Invoke(new DisconnectedDelegate(Disconnected), new object[] { sender, e });
            }
            */
        }

        private void Disconnected(object sender, EventArgs e)
        {
            /* 接続断処理 */
            Debug.WriteLine("Disconnected");
        }

            
        /** 接続OKイベント **/
        void tClient_OnConnected(EventArgs e)
        {
            /* 接続OK処理 */
            Debug.WriteLine("tClient_OnConnected");
        }
        
        /** データ受信イベント **/
        void tClient_OnReceiveData(object sender, string e)
        {
            /* 別スレッドからくるのでInvokeを使用 */
            /*
            if (this.InvokeRequired)
            {
                this.Invoke(new ReceiveDelegate(ReceiveData), new object[] { sender, e });
            }
            */
        }
        
        /* データ受信処理 */
        private void ReceiveData(object sender, string e)
        {
            Debug.WriteLine("ReceiveData:" + e + " ThreadID:");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            /* 送信 */
            //tcpClient.Send(txbxSendStr.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Form1_FormClosing" + " ThreadID:");
            if (!tcpClient.IsClosed)
            {
                tcpClient.Close();
            }
        }

        public void WorkSpace()
        {
            try
            {
                /* 接続処理 */
                /* Connect to the remote endpoint. */
                tcpClient.Connect(address, PORT);
            }
            catch (Exception e)
            {

            }
        }
    }
}
