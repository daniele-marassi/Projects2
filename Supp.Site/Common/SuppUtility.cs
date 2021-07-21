using Additional.NLog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NLog;
using Supp.Site.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();

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

        /// <summary>
        /// JsonToQuery
        /// </summary>
        /// <param name="jsonQuery"></param>
        /// <returns></returns>
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
        public void RemoveCookie(HttpResponse response, HttpRequest request, string key)
        {
            response.Cookies.Delete(key);
            if (ReadCookie(request, key) != String.Empty)
            {
                SetCookie(response, key, String.Empty, 0);
            }         
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

        /// <summary>
        /// AddErrorToCookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="error"></param>
        public void AddErrorToCookie(HttpRequest request, HttpResponse response, string error)
        {
            var errorsFromCookie = ReadCookie(request, GeneralSettings.Constants.SuppSiteErrorsCookieName);

            var errors = JsonConvert.DeserializeObject<List<string>>(errorsFromCookie);
            if (errors == null) errors = new List<string>() { };
            errors.Add(error);

            string errorsToJson = JsonConvert.SerializeObject(errors);

            SetCookie(response, GeneralSettings.Constants.SuppSiteErrorsCookieName, errorsToJson, 600);
        }

        /// <summary>
        /// SplitCamelCase
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GetClaims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ClaimsDto GetClaims(ClaimsPrincipal user)
        {
            var nLogUtility = new NLogUtility();
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var dto = new ClaimsDto() { IsAuthenticated = false, Roles = new List<string>() { } };

                try
                {
                    var claims = user.Claims.ToList();

                    if (claims != null && claims.Count > 0)
                    {
                        dto.IsAuthenticated = true;

                        dto.UserName = claims.Where(_ => _.Type == nameof(ClaimsDto.UserName)).Select(_ => _.Value).FirstOrDefault();

                        var configInJson = claims.Where(_ => _.Type == "ConfigInJson").Select(_ => _.Value).FirstOrDefault();
                        if (configInJson != null && configInJson != String.Empty)
                        {
                            var obj = JsonConvert.DeserializeObject<Configuration>(configInJson);
                            dto.Configuration = obj;
                        }
                        else dto.Configuration = JsonConvert.DeserializeObject<Configuration>(GeneralSettings.Static.ConfigDefaultInJson);

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
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    throw ex;
                }

                return dto;
            }
        }

        /// <summary>
        /// GetSalutation
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string GetSalutation(CultureInfo cultureInfo)
        {
            Random rnd = new Random();
            var result = "";
            var now = DateTime.Now;

            if (GetPartOfTheDay(now) == PartsOfTheDayEng.Morning)
            {
                int x = rnd.Next(0, 10 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buongiorno.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good morning.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona giornata.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice day.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = "Buona giornata.";
                if (cultureInfo.Name == "en-US" && x == 2) result = "Good day.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = ". Ti auguro una splendida giornata.";
                if (cultureInfo.Name == "en-US" && x == 3) result = ". Have a beautiful day.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = "Splendida giornata.";
                if (cultureInfo.Name == "en-US" && x == 4) result = "Beautiful day.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = ". Ti auguro una meravigliosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 5) result = ". Have a marvelous day.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = "Meravigliosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 6) result = "Marvelous day.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = ". Ti auguro una stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = ". Have a stupendous day.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = "Stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 8) result = "Stupendous day.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = ". Ti auguro una strepitosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 9) result = ". Have a amazing day.";

                if (cultureInfo.Name == "it-IT" && x == 10) result = "Strepitosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 10) result = "Amazing day.";
            }

            if (GetPartOfTheDay(now) == PartsOfTheDayEng.Afternoon)
            {
                int x = rnd.Next(0, 9 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buon pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro un buon pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = ". Ti auguro un splendido pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 2) result = ". Have a beautiful afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = "Splendido pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 3) result = "Beautiful afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = ". Ti auguro un meraviglioso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 4) result = ". Have a marvelous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = "Meraviglioso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 5) result = "Marvelous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = ". Ti auguro un stupendo pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 6) result = ". Have a stupendous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = "Stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = "Stupendous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = ". Ti auguro un strepitoso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 8) result = ". Have a amazing afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = "Strepitoso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 9) result = "Amazing afternoon.";
            }

            if (GetPartOfTheDay(now) == PartsOfTheDayEng.Evening)
            {
                int x = rnd.Next(0, 10 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buonasera.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good evening.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona serata.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice evening.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = "Buona serata.";
                if (cultureInfo.Name == "en-US" && x == 2) result = "Good evening.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = ". Ti auguro una splendida serata.";
                if (cultureInfo.Name == "en-US" && x == 3) result = ". Have a beautiful evening.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = "Splendida serata.";
                if (cultureInfo.Name == "en-US" && x == 4) result = "Beautiful evening.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = ". Ti auguro una meravigliosa serata.";
                if (cultureInfo.Name == "en-US" && x == 5) result = ". Have a marvelous evening.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = "Meravigliosa serata.";
                if (cultureInfo.Name == "en-US" && x == 6) result = "Marvelous evening.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = ". Ti auguro una stupenda serata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = ". Have a stupendous evening.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = "Stupenda serata.";
                if (cultureInfo.Name == "en-US" && x == 8) result = "Stupendous evening.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = ". Ti auguro una strepitosa serata.";
                if (cultureInfo.Name == "en-US" && x == 9) result = ". Have a amazing evening.";

                if (cultureInfo.Name == "it-IT" && x == 10) result = "Strepitosa serata.";
                if (cultureInfo.Name == "en-US" && x == 10) result = "Amazing evening.";
            }

            if (GetPartOfTheDay(now) == PartsOfTheDayEng.Night)
            {
                int x = rnd.Next(0, 9 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buona notte.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good night.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona notte.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice night.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = ". Ti auguro una splendida notte.";
                if (cultureInfo.Name == "en-US" && x == 2) result = ". Have a beautiful night.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = "Splendida notte.";
                if (cultureInfo.Name == "en-US" && x == 3) result = "Beautiful night.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = ". Ti auguro una meravigliosa notte.";
                if (cultureInfo.Name == "en-US" && x == 4) result = ". Have a marvelous night.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = "Meravigliosa notte.";
                if (cultureInfo.Name == "en-US" && x == 5) result = "Marvelous night.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = ". Ti auguro una stupenda notte.";
                if (cultureInfo.Name == "en-US" && x == 6) result = ". Have a stupendous night.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = "Stupenda notte.";
                if (cultureInfo.Name == "en-US" && x == 7) result = "Stupendous night.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = ". Ti auguro una strepitosa notte.";
                if (cultureInfo.Name == "en-US" && x == 8) result = ". Have a amazing night.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = "Strepitosa notte.";
                if (cultureInfo.Name == "en-US" && x == 9) result = "Amazing night.";
            }

            return result;
        }

        /// <summary>
        /// GetPartOfTheDay
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static PartsOfTheDayEng GetPartOfTheDay(DateTime dateTime)
        {
            var time = int.Parse(DateTime.Now.ToString("HHmm"));
            var result = PartsOfTheDayEng.NotSet;

            if (time >= 600 && time <= 1159) result = PartsOfTheDayEng.Morning;
            if (time >= 1200 && time <= 1759) result = PartsOfTheDayEng.Afternoon;
            if (time >= 1800 && time <= 2359) result = PartsOfTheDayEng.Evening;
            if (time >= 0 && time <= 559) result = PartsOfTheDayEng.Night;

            return result;
        }
    }
}