using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenceWorkorder.Models
{
    public class Client
    {
        public string clientName { get; set; }
        public string cpuId { get; set; }
        public List<string> macAddress { get; set; }
        public string ipAddress { get; set; }
    }
}
