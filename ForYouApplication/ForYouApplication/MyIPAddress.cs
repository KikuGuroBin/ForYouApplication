using System.Net;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class MyIPAddress
    {
        public static IPAddress GetIPAddress()
        {
            /* ListenするIPアドレス */
            IPAddress ipAdd = null;
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in addresses)
            {
                /* IPv4 のみを追加する */
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAdd = address;
                }
            }
            return ipAdd;
        }
    }
}
