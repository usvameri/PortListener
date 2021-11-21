using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LicenceWorkorder.Services
{
    public static class TcpMessager
    {
        public static void SendMessage()
        {
            try
            {
                Int32 port = 12989;
                IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
                var message = "Deneme";

                var client = new TcpClient(iPAddress.ToString(),port);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Log.Logger.Information(message, "Sent:");

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Log.Logger.Information(responseData, "received :");
                //Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Log.Logger.Warning(e, "Error:");
            }
            catch (SocketException e)
            {
                Log.Logger.Warning(e, "Error:");

            }
         
        }
	
    }
}
