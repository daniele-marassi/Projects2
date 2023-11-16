using System;
using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Webkit;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using Xamarin.Essentials;
using System.Threading;

namespace Supp.App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //var ip = "192.168.1.105:83";

            //var keyValuePairs = new Dictionary<string, string>() { };
            //keyValuePairs["api"] = "GetIp";
            //keyValuePairs["Token"] = "cf870b1832e928369b7872dd741906e4";
            //var apiParameters = new ApiParameters() { ParametresForUrlQuery = keyValuePairs };

            //GetIpResult result = null;

            //result = GetIP<GetIpResult>(HttpMethod.Get, "http://supp.altervista.org/", "Config.php", apiParameters).GetAwaiter().GetResult();

            //if (result != null && result.State == "Successful")
            //{
            //    ip = result?.Data?.FirstOrDefault()?.IP + ":16384";

            //    // Get our UI controls from the loaded layout
            Android.Webkit.WebView webView1 = FindViewById<WebView>(Resource.Id.WebView1);

            //webView1.SetWebViewClient(new WebViewClientOverride());
            //webView1.SetWebChromeClient(new WebChromeClientOverride());
            //webView1.Settings.JavaScriptEnabled = true;
            //webView1.Settings.AllowFileAccessFromFileURLs = true;
            //webView1.Settings.AllowUniversalAccessFromFileURLs = true;
            //webView1.Settings.AllowContentAccess = true;
            //webView1.Settings.AllowFileAccess = true;
            //webView1.Settings.MediaPlaybackRequiresUserGesture = false;
            //webView1.Settings.DatabaseEnabled = true;
            //webView1.ClearCache(true);
            //webView1.ClearHistory();

            //    webView1.Settings.JavaScriptCanOpenWindowsAutomatically = true;

            //    webView1.LoadUrl("https://" + ip + "/WebSpeeches/Recognition?_username=daniele.marassi.phone1@gmail.com&_password=Enilno.00&_application=false&_alwaysShow=false&_login=true");
            //}

            webView1.LoadUrl("http://supp.altervista.org/box.php?_username=daniele.marassi.phone1@gmail.com&_password=Enilno.00&_application=false&_alwaysShow=false&_login=true");

            //try
            //{
            //    Browser.OpenAsync("http://supp.altervista.org/box.php?_username=daniele.marassi.phone1@gmail.com&_password=Enilno.00&_application=false&_alwaysShow=false&_login=true", BrowserLaunchMode.SystemPreferred).GetAwaiter().GetResult();
            //}
            //catch (System.Exception ex)
            //{

            //}

            //Thread.Sleep(10000);

            //System.Diagnostics.Process.GetCurrentProcess().Kill();

            //Thread.CurrentThread.Abort();

            //System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            this.FinishAffinity();
        }
        
        private async Task<T> GetIP<T>(HttpMethod methodType, string baseUrl, string apiUrl, ApiParameters apiParameters = null, string token = null, string tokenType = null, int timeoutInMinutes = 10, bool skipCertificateValidation = false)
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
                var request = (HttpWebRequest)WebRequest.Create(new Uri(baseUrl) + apiUrl + DictionaryToQuery(apiParameters.ParametresForUrlQuery));
                request.Method = methodType.ToString();
                request.ContentType = "application/json";

                var webResponse = request.GetResponse();
                using (var _webStream = webResponse.GetResponseStream() ?? Stream.Null)
                {
                    using (var responseReader = new StreamReader(_webStream))
                    {
                        var content = responseReader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<T>(content);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return result;
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

        public class ApiParameters
        {
            public Dictionary<string, string> ParametresForUrlQuery { get; set; }
            public object Object { get; set; }
            public Dictionary<string, string> Headers { get; set; }
        }

        private string DictionaryToQuery(Dictionary<string, string> keyValuePairs)
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
