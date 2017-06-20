using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ForYouApplication
{
    /* Recive用構造体 */
    public class State
    {
        public Socket socket = null;
        /* 受信データのバッファーサイズ */
        public const int BufferSize = 256;
        /* 受信データ格納用 */
        public byte[] buffer = new byte[BufferSize];
        /* 受信データ結合用 */
        public StringBuilder sb = new StringBuilder();
    }
}
