using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Account
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Locale { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
        }

        public interface IFacebookClient
        {
            Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
            Task PostAsync(string accessToken, string endpoint, object data, string args = null);
        }

        public class FacebookClient : IFacebookClient
        {
            private readonly HttpClient _httpClient;

            public FacebookClient()
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
                };
                _httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
            {
                var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
                if (!response.IsSuccessStatusCode)
                    return default(T);

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(result);
            }

            public async Task PostAsync(string accessToken, string endpoint, object data, string args = null)
            {
                var payload = GetPayload(data);
                await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
            }

            private static StringContent GetPayload(object data)
            {
                var json = JsonConvert.SerializeObject(data);

                return new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{

            //    var fb = new FacebookClient();
            //    dynamic result = fb.Get("oauth/access_token", new
            //    {
            //        client_id = "---my application number---",
            //        client_secret = "---my application secret---",
            //        grant_type = "client_credentials"
            //    });

            //    var fb2 = new FacebookClient(result.access_token);
            //    dynamic albums = fb2.Get("my application number/albums");

            //    foreach (dynamic albumInfo in albums)
            //    {
            //        try
            //        {
            //            dynamic albumsPhotos = fb2.GetTaskAsync(albumInfo.id + "/photos");

            //        }
            //        catch (Exception Exception)
            //        {
            //            throw Exception;
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            var id = "332960710416200";

 public static string AccessToken = "EAACEdEose0cBAL7qx9UEFGqmYhBzml9SVLOIk62y7JFCwIaxZBnrZBcZCOPwRrc0COmC6PUZBg5ZBpZBrZCSXIiZCWWZA3gFWF5tZAbPwUa7cRVhuZAIETacQhER2vvqCS6yUA2wZAnsN1dK7DKJhp1sUZCRCLrocJlCMgyt4FjZBlrmzA0PJxKfHXwNNlHMjLyeZAoZARGiD68oertnOQZDZD";


        var profilePic = getUrlImage("https://graph.facebook.com/" + id + "/picture");


            //http://graph.facebook.com/{albumid}/photos?access_token={token}

            pictureBox1.Image = profilePic;
            var ttt =0;
        }

        private Image getUrlImage(string url)
        {
            WebResponse result = null;
            Image rImage = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                result = request.GetResponse();
                Stream stream = result.GetResponseStream();
                BinaryReader br = new BinaryReader(stream);
                byte[] rBytes = br.ReadBytes(1000000);
                br.Close();
                result.Close();
                MemoryStream imageStream = new MemoryStream(rBytes, 0, rBytes.Length);
                imageStream.Write(rBytes, 0, rBytes.Length);
                rImage = Image.FromStream(imageStream, true);
                imageStream.Close();
            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
            }
            finally
            {
                if (result != null) result.Close();
            }
            return rImage;

        }
    }
}
