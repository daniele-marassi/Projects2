
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Mair.DS.Client.Web.Common
{
    public class Utility
    {
        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="keyValuePairs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(HttpMethod methodType, string baseUrl, string apiUrl, Dictionary<string, string> keyValuePairs = null, string token = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                //init
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(10);

                var par = "";

                //add parameters in Uri
                if (((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Get || methodType == HttpMethod.Delete)))
                {
                    par = JsonToQuery(JsonConvert.SerializeObject(keyValuePairs));
                }

                //create request
                var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                if (token != null && token != String.Empty)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                //add parameters in headers
                if ((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Post || methodType == HttpMethod.Put))
                {
                    var content = new FormUrlEncodedContent(keyValuePairs);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    request.Content = content;
                }
                //Console.WriteLine(request);
                //call
                response = await client.SendAsync(request);
            }

            return response;
        }

        public string JsonToQuery(string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }

        /// <summary>
        /// Get Md5 Hash
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify Md5 Hash
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}