using Google.Apis.Calendar.v3;
using Google.Apis.Keep.v1;
using Google.Apis.Keep.v1.Data;
using Google.Apis.Services;
using GoogleManagerModels;
using GoogleService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            var authInJson = @"{""installed"":{""client_id"":""982569746577-4lnrb3udcu2dqqk2u2mts0j2rqmiripd.apps.googleusercontent.com"",""project_id"":""ace-case-311111"",""auth_uri"":""https://accounts.google.com/o/oauth2/auth"",""token_uri"":""https://oauth2.googleapis.com/token"",""auth_provider_x509_cert_url"":""https://www.googleapis.com/oauth2/v1/certs"",""client_secret"":""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"",""redirect_uris"":[""urn:ietf:wg:oauth:2.0:oob"",""http://localhost""]}}";
            var auth = JsonConvert.DeserializeObject<Auth>(authInJson);

            var tokenFileInJson = @"{""FileName"":""Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi"",""Content"":""{\""access_token\"":\""ya29.a0ARrdaM_hEf2somCePpt5n4t5gtn3NApqpM2umG1bWLnIRXkjRv61zb_d-4tsdFM53MM0-70lTziBn-i_r6f8WoqxUnwQ7DTWyqLm4VJxCeof6X0iVQMK0yROQFXdzHNtaj3e1ujRZObABFFNT1PZ6yOIHrER\"",\""token_type\"":\""Bearer\"",\""expires_in\"":3599,\""refresh_token\"":\""1//092bvmBG_sMJqCgYIARAAGAkSNwF-L9IrTv6wmWF3fTpC7mfSMao7wTbk7LsK8--mXsMMrxIwywC1vdA8_3McbiB_vzS8bxmkeUg\"",\""scope\"":\""https://www.googleapis.com/auth/calendar.readonly https://www.googleapis.com/auth/calendar\"",\""Issued\"":\""2021-12-01T14:44:01.710+01:00\"",\""IssuedUtc\"":\""2021-12-01T13:44:01.710Z\""}""}";
            var tokenFile = JsonConvert.DeserializeObject<TokenFile>(tokenFileInJson);

            var googlePublicKey = "AIzaSyCdWVUdy3QmmYLjDwQWqP03gV49hfvWMhc";

            var getNotesRequest = new NotesRequest()
            {
                Auth = auth,
                TokenFile = tokenFile,
                Account = "daniele.marassi",
                TimeMin = DateTime.Parse("2021-12-01 00:00:00"),
                TimeMax = DateTime.Parse("2021-12-01 23:59:59")
            };

            var notes = GetNotes(getNotesRequest);

            //foreach (var item in notes)
            //{
            //    //listBox1.Items.Add(item);
            //    var tt = 0;
            //}
        }

        public KeepService GetService(TokenFile tokenFile, Auth auth, string account)
        {
            var result = new KeepService();

            try
            {
                var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var accessProperties = JsonConvert.DeserializeObject<AccessProperties>(tokenFile.Content);
                accessProperties.Expires_in = 3599;

                var resourcesPath = Path.Combine(appPath, "Resources");
                if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
                var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                var tokenFileFullPath = Path.Combine(tokenFilePath, tokenFile.FileName);

                if (System.IO.File.Exists(tokenFileFullPath)) System.IO.File.Delete(tokenFileFullPath);

                System.IO.File.WriteAllText(tokenFileFullPath, JsonConvert.SerializeObject(accessProperties));

                var googleServiceUtility = new GoogleServiceUtility();

                var managerRequest = new ManagerRequest() { Auth = auth, Account = account };

                var service = new KeepService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleServiceUtility.CreateCredential(managerRequest, AccountType.Note),
                    ApplicationName = "Google Note",
                });

                result = service;
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public List<ListNotesResponse> GetNotes(NotesRequest getNotesRequest)
        {
            var result = new List<ListNotesResponse>() { };

            try
            {
                var _service = GetService(getNotesRequest.TokenFile, getNotesRequest.Auth, getNotesRequest.Account);

                var _request = _service.Notes.List();

                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Notes != null && events.Notes.Count > 0)
                {
                    foreach (var item in events.Notes)
                    {
                        var yyy = 0;
                    }
                }
                else
                {
                    //"No upcoming events found."
                    result = new List<ListNotesResponse>() { };
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
    }
}
