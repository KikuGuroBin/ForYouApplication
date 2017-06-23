using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace ForYouApplication
{
    /* ホストからの切断要求を受け取るための受信待ちクラス */
    public class AsyncReceiver
    {
        private const string DEFAULTRECEIVEDATA = "NONE";
        private const string ENDCONNECTION = "<ENDCONNECTION>"; 

        private AsyncTcpClient client;

        /* コンストラクタ */
        public AsyncReceiver(AsyncTcpClient client)
        {
            this.client = client;
        }

        /* 非同期でホストからの受信待ちをし、受信内容に応じて処理を行う */
        public async void ReceiveTask()
        {
            /* 人工的な無限ループのスレッド */
            await Task.Run(() =>
            {
                while (true)
                {
                    /* ホストからの受信データ取得 */
                    string data = client.Receive();

                    /* 受信したデータがない場合 */
                    if (data.Equals(DEFAULTRECEIVEDATA))
                    {
                        continue;
                    }
                    /* ホストから切断要求があった場合 */
                    else if (data.IndexOf(ENDCONNECTION) > -1)
                    {
                        client.DisConnect();
                    }
                    else
                    {

                    }
                }
            });
        }
    }
}