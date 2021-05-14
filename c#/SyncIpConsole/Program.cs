using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyncIp
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "";
            var oldAddress = "";
            var utility = new Utility();
            while (true)
            {
                address = utility.GetIPAddress();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + address);
                if (address != oldAddress)
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("api", "SetIp"));
                    keyValuePairs.Add(new KeyValuePair<string, string>("Token", "cf870b1832e928369b7872dd741906e4"));
                    keyValuePairs.Add(new KeyValuePair<string, string>("Ip", address));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = utility.CallApi(HttpMethod.Post, "http://supp.altervista.org/", "Config.php", parameters, null).Result;
                    var content = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + content);
                    var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    var message = obj["Message"];
                }
                oldAddress = address;
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
