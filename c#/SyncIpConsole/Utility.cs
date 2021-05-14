using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SyncIp
{
    public class Utility
    {
        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="contentToParametersOrValues"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(HttpMethod methodType, string baseUrl, string apiUrl, FormUrlEncodedContent contentToParametersOrValues, string token)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (token != null && token != String.Empty) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.Timeout = TimeSpan.FromMinutes(10);

                var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl);

                contentToParametersOrValues.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                request.Content = contentToParametersOrValues;

                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Get IP Address
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }
    }
}
