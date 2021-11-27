using LicenceWorkorder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LicenceWorkorder.Services
{
    public static class RequestService
    {
        public static object GetSoftwareParts(string username, string password)
        {
            var httpClient = new HttpClient();
            var uri = new Uri("https://localhost:5001/account/programlogin");
            var json = JsonSerializer.Serialize(new { username = username, password = password });
            var stringContent = new StringContent(json);
            var result = httpClient.PostAsync(uri, stringContent).Result.Content.ReadAsStringAsync();
            if (result != null)
            {
                var softwareParts = JsonSerializer.Deserialize<List<ProductSoftwarePart>>(result.Result);
                return softwareParts;
            }
            return null;

        }
    }
}
