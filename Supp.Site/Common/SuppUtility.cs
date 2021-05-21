using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Supp.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;

namespace Supp.Site.Common
{
    public class SuppUtility
    {
        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="parametersJsonString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(HttpMethod methodType, string baseUrl, string apiUrl, string parametersJsonString, string token)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                //init
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(10);

                var par = "";

                if ((parametersJsonString != null && parametersJsonString != String.Empty) && (methodType == HttpMethod.Get || methodType == HttpMethod.Delete))
                {
                    par = JsonToQuery(parametersJsonString);
                }

                //create request
                var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (token != null && token != String.Empty)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                request.Content = null;
                //add parameters
                if ((parametersJsonString != null && parametersJsonString != String.Empty) && (methodType == HttpMethod.Post || methodType == HttpMethod.Put))
                {
                    request.Content = new StringContent(parametersJsonString);
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
            var errorsFromCookie = ReadCookie(request, GeneralSettings.Constants.SuppSiteErrorsCookieName);

            var errors = JsonConvert.DeserializeObject<List<string>>(errorsFromCookie);
            if (errors == null) errors = new List<string>() { };
            errors.Add(error);

            string errorsToJson = JsonConvert.SerializeObject(errors);

            SetCookie(response, GeneralSettings.Constants.SuppSiteErrorsCookieName, errorsToJson, 600);
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

        public static ClaimsDto GetClaims(ClaimsPrincipal user)
        {
            var claims = user.Claims.ToList();

            var dto = new ClaimsDto() { IsAuthenticated = false, Roles = new List<string>() { } };

            if (claims != null && claims.Count > 0)
            {
                dto.IsAuthenticated = true;

                dto.UserName = claims.Where(_ => _.Type == nameof(ClaimsDto.UserName)).Select(_ => _.Value).FirstOrDefault();

                var configInJson = claims.Where(_ => _.Type == nameof(ClaimsDto.Configuration)).Select(_ => _.Value).FirstOrDefault();
                if (configInJson != null && configInJson != String.Empty)
                {
                    var obj = JsonConvert.DeserializeObject<Configuration>(configInJson);
                    dto.Configuration = obj;
                }

                dto.Name = claims.Where(_ => _.Type == nameof(ClaimsDto.Name)).Select(_ => _.Value).FirstOrDefault();
                dto.Surname = claims.Where(_ => _.Type == nameof(ClaimsDto.Surname)).Select(_ => _.Value).FirstOrDefault();
                dto.UserId = long.Parse(claims.Where(_ => _.Type == nameof(ClaimsDto.UserId)).Select(_ => _.Value).FirstOrDefault());

                var rolesInString = claims.Where(_ => _.Type == nameof(ClaimsDto.Roles)).Select(_ => _.Value).FirstOrDefault();
                if (rolesInString != null && rolesInString != String.Empty)
                {
                    var obj = rolesInString.Split(",");
                    dto.Roles.AddRange(obj);
                }

            }

            return dto;
        }
    }
}