using LicenceWorkorder.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LicenceWorkorder.Services
{
    public class ClientWorker
    {
        public Client CheckClient()
        {
            var client = new Client();
            ManagementClass managementClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managementBaseObjects = managementClass.GetInstances();
            var macAddresses = new List<string>();
            foreach (ManagementObject mo in managementBaseObjects)
            {
                client.cpuId = mo.Properties["processorId"].Value.ToString();
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        PhysicalAddress Mac = nic.GetPhysicalAddress();
                        macAddresses.Add(Mac.ToString());
                    }
                }
                //var revision = managementObject.Properties["Revision"].Value.ToString();

                //Console.WriteLine(revision);
            }


            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
                client.ipAddress = localIP;
            }


            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            client.clientName = userName;
            client.macAddress = macAddresses;
            var json = JsonSerializer.Serialize(client);
            var textforsave = client.cpuId + "|" + client.clientName + "|" + client.ipAddress;
            string securityKey = "tubbiyaSecurity";
            var result = StringChipher.Encrypt(textforsave, securityKey);
            Log.Logger.Information(result);
            Log.Logger.Information("-------------------------------");
            //Console.WriteLine("-------------------------------");
            var decrypt = StringChipher.Decrypt(result, securityKey);
            //Console.WriteLine(decrypt);
            Log.Logger.Information(decrypt);


            return client;
        }

    }
}
