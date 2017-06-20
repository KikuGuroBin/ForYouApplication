using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;

namespace ForYouApplication
{
    public class Connection3
    {
        async public void Binder(string server)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.

                Debug.WriteLine("---------------------接続中---------------------");

                TcpClient client = new TcpClient(server, 55555);
            
                Debug.WriteLine("---------------------（接続）OK！---------------------");

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.UTF8.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.UTF8.GetString(data, 0, bytes);
                Debug.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: {0}", e);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
