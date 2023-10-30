using Additional.NLog;
using GoogleManagerModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Supp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static Supp.Site.Common.Config;

namespace Supp.Site.Common
{
    public class SuppUtility
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();

        /// <summary>
        /// Set Cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireInSeconds"></param>
        /// <param name="onlySpecificKey"></param>
        public void SetCookie(HttpResponse response, string key, string value, int? expireInSeconds, bool onlySpecificKey = false)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddSeconds((double)expireInSeconds);

            if (value == null) value = String.Empty;

            if (onlySpecificKey == false)
            {
                value = HttpUtility.UrlEncode(value);

                var cookieCharacterLimit = (4096 - key.Length) - 100;
                var i = 0;

                while (value != String.Empty)
                {
                    cookieCharacterLimit = cookieCharacterLimit - i.ToString().Length - 1;
                    var _value = "";
                    if (value.Length < cookieCharacterLimit) _value = value.Substring(0, value.Length);
                    else _value = value.Substring(0, cookieCharacterLimit);

                    value = value.Substring(_value.Length, value.Length - _value.Length);

                    _value = HttpUtility.UrlDecode(_value);

                    response.Cookies.Append(key + "_" + i.ToString(), _value, option);

                    i++;
                }
            }
            else
            {
                response.Cookies.Append(key, value, option);
            }
        }

        /// <summary>
        /// Read Cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="onlySpecificKey"></param>
        /// <returns></returns>
        public string ReadCookie(HttpRequest request, string key, bool onlySpecificKey = false)
        {
            var value = "";

            if (onlySpecificKey == false)
            {
                var i = 0;
                var _key = key + "_" + i.ToString();

                while (request != null && request.Cookies[_key] != null)
                {
                    value += request.Cookies[_key];
                    i++;
                    _key = key + "_" + i.ToString();
                }
            }
            else
            {
                value = request.Cookies[key];
            }

            if(value != null) value = HttpUtility.UrlDecode(value);

            return value;
        }

        /// <summary>
        /// Remove Cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="onlySpecificKey"></param>
        public void RemoveCookie(HttpResponse response, HttpRequest request, string key, bool onlySpecificKey = false)
        {
            if (onlySpecificKey == false)
            {
                var i = 0;
                var _key = key + "_" + i.ToString();

                while (request != null && request.Cookies[_key] != null)
                {
                    response.Cookies.Delete(_key);
                    i++;
                    _key = key + "_" + i.ToString();
                }

                i = 0;
                _key = key + "_" + i.ToString();

                while (request != null && request.Cookies[_key] != null)
                {
                    SetCookie(response, _key, String.Empty, 0, true);
                    i++;
                    _key = key + "_" + i.ToString();
                }
            }
            else
            {
                response.Cookies.Delete(key);
                if(request != null && request.Cookies[key] != null)
                    SetCookie(response, key, String.Empty, 0, true);
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

        public static long GetUserIdFromToken(string token)
        {
            long userId = 0;
            try
            {
                if (token == null) token = "";

                long.TryParse(token.Split("|")[0], out userId);
            }
            catch (Exception)
            {

            } 
            
            return userId;
        }

        /// <summary>
        /// Get Identification
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static TokenDto GetIdentification(HttpRequest request, long userId)
        {
            var nLogUtility = new NLogUtility();
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var dto = new TokenDto() { IsAuthenticated = false, Roles = new List<string>() { } };
                try
                {
                    var suppUtility = new SuppUtility();

                    if (userId > 0)
                    {
                        var token = "";

                        token = suppUtility.GetAccessToken(request);

                        if (token == null) token = "";

                        userId = GetUserIdFromToken(token);
                    }

                    if (Program.TokensArchive.ContainsKey(userId))
                        dto = Program.TokensArchive[userId];
                    else
                    {
                        var identification = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteTokenDtoCookieName);
                        if (identification != null && identification != "")
                        {
                            var token = "";

                            token = suppUtility.GetAccessToken(request);

                            if (token != null && token != "")
                            {

                                userId = GetUserIdFromToken(token);

                                try
                                {
                                    Program.TokensArchive[userId] = JsonConvert.DeserializeObject<TokenDto>(identification);

                                    dto = Program.TokensArchive[userId];
                                }
                                catch (Exception)
                                {
                                }
                            }
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

        public static void TokenStorage(TokenDto dto, NLogUtility nLogUtility)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    if (Program.TokensArchive.ContainsKey(dto.UserId))
                    {
                        TokenDto tokenDtoDeleted = null;
                        Program.TokensArchive.TryRemove(dto.UserId, out tokenDtoDeleted);
                    }

                    Program.TokensArchive.TryAdd(dto.UserId, dto);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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

        /// <summary>
        /// Get Answer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public string GetAnswer(string value, TokenDto identification)
        {
            var rnd = new Random();
            var list = new List<string>() { };

            try
            {
                list = JsonConvert.DeserializeObject<List<string>>(value);
            }
            catch (Exception)
            {
                list.Add(value);
            }

            if (list == null) list = new List<string>() { "" };

            var x = rnd.Next(0, list.Count());

            value = list[x];

            if (value == null) value = "";

            value = value.Replace("SURNAME", identification.Surname);
            value = value.Replace("NAME", identification.Name);

            return value;
        }

        public static T Clone<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return (T)JsonConvert.DeserializeObject<T>(json);
        }

        public string GetAccessToken(HttpRequest request)
        {
            var access_token_cookie = "";
            long userId = 0;
            long.TryParse(ReadCookie(request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName), out userId);

            if (Program.TokensArchive != null && Program.TokensArchive.ContainsKey(userId))
            {
                var tokenDto = Program.TokensArchive[userId];

                if (tokenDto != null)
                {
                    access_token_cookie = tokenDto?.TokenCode;
                }
            }

            if(access_token_cookie == "" || access_token_cookie == null)
                access_token_cookie = ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

            return access_token_cookie;
        }
    }
}