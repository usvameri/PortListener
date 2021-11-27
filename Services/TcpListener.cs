using LicenceWorkorder.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public void HttpListener(string port)
        {

            HttpListener listener = new HttpListener();
            var url = string.Format("http://127.0.0.1:{0}/", port);

            listener.Prefixes.Add(url);

            try
            {
                listener.Start();
                Log.Logger.Warning($"Listening on {url}");
            }
            catch (HttpListenerException hlex)
            {
                Log.Logger.Warning(hlex.Message);
            }
            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                Log.Logger.Information("Client Connected!");
                string content;
                using (var reader = new StreamReader(request.InputStream,
                                                     request.ContentEncoding))
                {
                    content = reader.ReadToEnd();
                }
                if (content != null && content != "")
                {
                    user user = JsonSerializer.Deserialize<user>(content);

                    //var httpClient = new HttpClient();
                    //var uri = new Uri("https://localhost:5001/account/programlogin");
                    //var json = JsonSerializer.Serialize(new { username = username, password = password });
                    //var stringContent = new StringContent(json);
                    //var result = httpClient.PostAsync(uri, stringContent).Result.Content.ReadAsStringAsync();
                    //if (result != null)
                    //{
                    //    var softwareParts = JsonSerializer.Deserialize<List<ProductSoftwarePart>>(result.Result);
                    //}
                    Log.Logger.Information(content);
                    //var softwareParts = GetSoftwareParts(user.username, user.password, user.cpuId,user.clientName).Result;
                    var softwareParts = GetSoftwareParts(user.username, user.password).Result;
                    Log.Logger.Information((string)softwareParts);


                }
            }
        }

        //public async Task<object> GetSoftwareParts(string username, string password, string cpuId, string clientName)
        public async Task<object> GetSoftwareParts(string username, string password)
        {
            var httpClient = new HttpClient();
            var uri = new Uri("https://localhost:5001/account/programlogin");
            var client = ClientInfo.CheckClient();
            var json = JsonSerializer.Serialize(new { username = username, password = password, cpuId= client.cpuId, clientName = client.clientName });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                if(result != null || result != "")
                {
                    //var softwareParts = JsonSerializer.Deserialize<List<ProductSoftwarePart>>(result);
                    var softwareParts = JsonSerializer.Deserialize<string>(result);
                    //string content = "";
                    //foreach (var item in softwareParts)
                    //{
                    //    content += item.name + "|";
                    //}
                    return softwareParts;
                }
            }
            return null;

        }
    }
}
