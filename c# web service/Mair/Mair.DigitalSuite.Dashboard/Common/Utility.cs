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
using static Mair.DigitalSuite.Dashboard.Common.Config;

namespace Mair.DigitalSuite.Dashboard.Common
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
        /// Set Cookie 
        /// </summary>
        /// <param name="response"> HttpResponse </param>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireInSeconds">expire In Seconds</param>
        public void SetCookie(HttpResponse response, string key, string value, int? expireInSeconds)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddSeconds((double)expireInSeconds);
            response.Cookies.Append(key, value, option);
        }

        /// <summary>
        /// Read Cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadCookie(HttpRequest request, string key)
        {
            string value = String.Empty;
            if (request != null && request.Cookies[key] != null) value = request.Cookies[key];
            return value;
        }

        /// <summary>
        /// Remove Cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="key"></param>
        public void RemoveCookie(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
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

        public void AddErrorToCookie(HttpRequest request, HttpResponse response, string error)
        {
            var errorsFromCookie = ReadCookie(request, GeneralSettings.Constants.MairDigitalSuiteErrorsCookieName);

            var errors = JsonConvert.DeserializeObject<List<string>>(errorsFromCookie);
            if (errors == null) errors = new List<string>() { };
            errors.Add(error);

            string errorsToJson = JsonConvert.SerializeObject(errors);

            SetCookie(response, GeneralSettings.Constants.MairDigitalSuiteErrorsCookieName, errorsToJson, 600);
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