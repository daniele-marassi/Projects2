using Additional;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Program
    {

        //public const int KEYEVENTF_EXTENTEDKEY = 1;
        //public const int KEYEVENTF_KEYUP = 0;
        //public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        //public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        //public const int VK_MEDIA_PREV_TRACK = 0xB1;
        //
        //[DllImport("user32.dll")]
        //public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        static void Main(string[] args)
        {
            //keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);

            var utility = new Additional.Utility();

            //var result = utility.RunExe(@"C:\Program Files (x86)\Commands\Media\MediaPlayOrPause.exe", "", false).GetAwaiter().GetResult();
            //"C:\Users\ev\Downloads\spotifylaunch.vbs" spotify:user:dm:playlist:3W4uVzChUd70KYwpAJEBDo
            //var result = utility.RunExe(@"C:\Users\ev\Downloads\spotifylaunch.vbs", "spotify:user:dm:playlist:3W4uVzChUd70KYwpAJEBDo", false).GetAwaiter().GetResult();

            //var ttt = RunExe(@"C:\Users\ev\Downloads\spotifylaunch.vbs", "spotify:user:dm:playlist:3W4uVzChUd70KYwpAJEBDo", false, true).GetAwaiter().GetResult();

            //Process process = new Process();
            //ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = @"C:\Users\ev\Downloads\spotifylaunch.vbs";
            //start.Arguments = "spotify:user:dm:playlist:3W4uVzChUd70KYwpAJEBDo";
            //process.StartInfo = start;
            //process.Start();

            var keyValuePairs = new Dictionary<string, string>() { };
            keyValuePairs["api"] = "GetIp";
            keyValuePairs["Token"] = "cf870b1832e928369b7872dd741906e4";
            //keyValuePairs["Ip"] = address;

            var apiParameters = new ApiParameters() { ParametresForUrlQuery = keyValuePairs };

            //CallApi<T>(methodType, baseUrl, apiUrl, apiParameters, string token = null, string tokenType = null, int timeoutInMinutes = 10, bool skipCertificateValidation = false)
            var result = CallApi<Dictionary<string, dynamic>>(HttpMethod.Get, "http://supp.altervista.org/", "Config.php", apiParameters, null).GetAwaiter().GetResult();

            var ip = result["Data"][0]["IP"].ToString();
            //var ip = result?.Data?.FirstOrDefault()?.IP;
        }

        public class GetIpResult
        {
            public string State { get; set; }
            public string Operation { get; set; }
            public string Message { get; set; }
            public List<Data> Data { get; set; }
        }

        public class Data
        {
            public string IP { get; set; }
        }

        public static async Task<(string Output, string Error)> RunExe(string fullPath, string arguments, bool async, bool useShellExecute = false)
        {
            (string Output, string Error) result;
            result.Output = null;
            result.Error = null;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = fullPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = useShellExecute;
                process.StartInfo.RedirectStandardOutput = async;
                process.StartInfo.RedirectStandardError = async;
                process.Start();

                if (async)
                {
                    result.Output = process.StandardOutput.ReadToEnd();

                    result.Error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            finally
            {
                if (result.Output != null && result.Output != String.Empty) Console.WriteLine("Output: " + result.Output);
                if (result.Error != null && result.Error != String.Empty) Console.WriteLine("Error: " + result.Error);
            }

            if (result.Output != null) result.Output = result.Output.Replace(System.Environment.NewLine, "");
            if (result.Error != null) result.Error = result.Error.Replace(System.Environment.NewLine, "");

            return result;
        }

        public class ApiParameters
        {
            public Dictionary<string, string> ParametresForUrlQuery { get; set; }
            public object Object { get; set; }
            public Dictionary<string, string> Headers { get; set; }
        }

        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="apiParameters"></param>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        /// <param name="timeoutInMinutes"></param>
        /// <param name="skipCertificateValidation"></param>
        /// <returns></returns>
        public static async Task<T> CallApi<T>(HttpMethod methodType, string baseUrl, string apiUrl, ApiParameters apiParameters = null, string token = null, string tokenType = null, int timeoutInMinutes = 10, bool skipCertificateValidation = false)
        {
            T result;

            if (skipCertificateValidation)
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var handler = new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //init
                    client.BaseAddress = new Uri(baseUrl);
                    client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);

                    var par = "";

                    //add parameters in Uri
                    if (apiParameters != null && apiParameters.ParametresForUrlQuery != null && apiParameters.ParametresForUrlQuery.Count > 0)
                    {
                        par = DictionaryToQuery(apiParameters.ParametresForUrlQuery);
                    }

                    //create request
                    var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                    if (token != null && token != System.String.Empty)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    }

                    //add parameters in headers
                    if (apiParameters != null && apiParameters.Object != null)
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(apiParameters.Object), Encoding.UTF8, "application/json");

                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        request.Content = content;
                    }

                    //add additional headers
                    if (apiParameters != null && apiParameters.Headers != null)
                    {
                        foreach (var header in apiParameters.Headers)
                        {
                            try
                            {
                                request.Headers.Add(header.Key, header.Value);
                            }
                            catch (System.Exception)
                            {
                            }
                        }
                    }

                    //call
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new System.Exception($"Call Api Error - ReasonPhrase:{response.ReasonPhrase}, StatusCode:{response.StatusCode}");
                    }
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        result = JsonConvert.DeserializeObject<T>(content);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Call Api Error - " + ex.Message, ex.InnerException);
            }

            return result;
        }


        private static string DictionaryToQuery(Dictionary<string, string> keyValuePairs)
        {
            var result = "";
            var query = "";

            foreach (var kvp in keyValuePairs)
            {
                if (query != "") query += "&";
                query += $"{kvp.Key}={kvp.Value}";
            }

            if (query != "") result = "?" + query;

            return result;
        }
    }
}
