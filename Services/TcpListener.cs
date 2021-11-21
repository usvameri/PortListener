using LicenceWorkorder.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LicenceWorkorder.Services
{
    public class TcpListener
    {
        public async void ListenTcpPort(BackgroundService worker, CancellationToken cancelletionToken)
        {
            //TcpClient server = new TcpClient();

            Int32 port = 12989;
            IPAddress localAdress = IPAddress.Parse("127.0.0.1");
            var listener2 = new HttpListener();
            

            var listener = new System.Net.Sockets.TcpListener(localAdress,port);
            
            try
            {
                listener.Start();
                Byte[] bytes = new Byte[256];
                String data = null;
                while (true)
                {
                    Log.Logger.Information("Waiting for client to connect..");
                    
                    var client =  listener.AcceptTcpClient();
                    Log.Logger.Information("Client Connected!");
                    //TcpMessager.SendMessage();

                    data = null;
                    int i;
                    NetworkStream stream = client.GetStream();

                    while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Log.Logger.Information(data);
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Log.Logger.Information("Sent: {0}", data);
                    }
                    client.Close();
                    //var clientStream = client.GetStream();

                }
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex,"Error");
            }
            finally
            {
                listener.Stop();
            }

            //await server.ConnectAsync(localAdress, port);
            //server.


        }

        public user HttpListener(string port)
        {

            HttpListener listener = new HttpListener();
            var url = string.Format("http://127.0.0.1:{0}/", port);

            listener.Prefixes.Add(url);

            try
            {
                listener.Start();
                Log.Logger.Information("Listener Started!");
                while (true)
                {
                    Log.Logger.Information("Client Connected!");
                    var context = listener.GetContext();
                    var request = context.Request;
                    string content;
                    using (var reader = new StreamReader(request.InputStream,
                                                         request.ContentEncoding))
                    {
                        content = reader.ReadToEnd();
                    }
                    if (content != null)
                    {
                        user user = JsonSerializer.Deserialize<user>(content);
                        return user;
                    }
                }
            }
            catch (HttpListenerException hlex)
            {
                Log.Logger.Warning(hlex.Message);
                return null;
            }
           
        }


    }
}
