using Additional;
using GoogleDriveCreateCredentials.Models;
using GoogleDriveManagerModels;
using GoogleDriveService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriveCreateCredentials
{
    public partial class GoogleDriveCreateCredentialsForm : Form
    {
        private static CredentialsParam credentialsParam = new CredentialsParam() { Data = new List<Credential>() { } };
        private static Utility utility = new Utility();
        public GoogleDriveCreateCredentialsForm()
        {
            InitializeComponent();
        }

        private void EnableTheDriveAPIBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://developers.google.com/drive/api/v3/quickstart/dotnet?pli=1");
        }

        private void AddCredential()
        {
            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var resourcesPath = Path.Combine(appPath, "Resources");
            var downloadsPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            downloadsPath = Path.Combine(downloadsPath, "Downloads");
            if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
            if (GoogleDriveAccountTxt.Text == null || GoogleDriveAccountTxt.Text == String.Empty)
            {
                MessageBox.Show("Insert Account");
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = downloadsPath;
                openFileDialog.FileName = "credentials.json";
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var account = GoogleDriveAccountTxt.Text.Split('@')[0];

                        var credentialsText = File.ReadAllText(openFileDialog.FileName);

                        var auth = JsonConvert.DeserializeObject<Auth>(credentialsText);
                        var googleDriveServiceUtility = new GoogleDriveServiceUtility();
                        var request = new ManagerRequest() { Auth = auth, Account = account };
                        var _service = googleDriveServiceUtility.CreateService(request);

                        var tokenResponseFileName = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-" + account;
                        var tokenResponseFullPath = Path.Combine(resourcesPath, @".credentials\apiName", tokenResponseFileName);

                        var tokenResponseText = File.ReadAllText(tokenResponseFullPath);

                        var tokenResponse = JsonConvert.DeserializeObject<AccessProperties>(tokenResponseText);

                        credentialsParam.Data.Add( 
                            new Credential() { 
                                Auth = auth, 
                                AccessProperties = tokenResponse, 
                                AccessFileName = tokenResponseFileName,
                                Account = account,
                                FolderToFilter = FolderToFilterTxt.Text
                            }
                        );

                        MessageBox.Show("Credential added!");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }

        private async Task SendCredentials()
        {
            try
            {
                var tokenResult = new TokenResult() { };
                var keyValuePairs = new Dictionary<string, string>() { };
                keyValuePairs["userName"] = UserNameTxt.Text;
                keyValuePairs["password"] = PasswordTxt.Text;

                var result = await utility.CallApi(HttpMethod.Get, AddressTxt.Text, "api/Authentications/GetToken", keyValuePairs, null);

                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode == false)
                {
                    Debug.WriteLine(result.ReasonPhrase);
                    MessageBox.Show(result.ReasonPhrase);
                    return;
                }
                else
                {
                    tokenResult = JsonConvert.DeserializeObject<TokenResult>(content);
                    if (!tokenResult.Successful)
                    {
                        Debug.WriteLine(tokenResult.Message);
                        MessageBox.Show(tokenResult.Message);
                        return;
                    }
                }

                foreach (var item in credentialsParam.Data)
                {
                    item.UserId = tokenResult.Data.FirstOrDefault().UserId;
                }

                keyValuePairs = new Dictionary<string, string>() { };
                keyValuePairs[""] = JsonConvert.SerializeObject(credentialsParam);

                result = await  utility.CallApi(HttpMethod.Post, AddressTxt.Text, "api/GoogleDriveAccounts/AddGoogleDriveCredentials", keyValuePairs, tokenResult.Data?.FirstOrDefault()?.Token);
                content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode == false)
                {
                    Debug.WriteLine(result.ReasonPhrase);
                    MessageBox.Show(result.ReasonPhrase);
                    return;
                }
                else
                {
                    var _result = JsonConvert.DeserializeObject<CredentialResult>(content);
                    if (!_result.Successful)
                    {
                        Debug.WriteLine(_result.Message);
                        MessageBox.Show(_result.Message);
                        return;
                    }
                    else if (_result.ResultState != Models.ResultType.Created)
                    {
                        Debug.WriteLine(_result.Message);
                        MessageBox.Show(_result.Message);
                        return;
                    }
                    else
                    {
                        credentialsParam = new CredentialsParam() { Data = new List<Credential>() { } };
                        MessageBox.Show(_result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            credentialsParam = new CredentialsParam() { Data = new List<Credential>() { } };
            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var resourcesPath = Path.Combine(appPath, "Resources");

            System.IO.DirectoryInfo dirInfo = new DirectoryInfo(resourcesPath);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
                
            }
            this.Enabled = true;
            MessageBox.Show("Reset executed!");
        }

        private async void SendCredentialsBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            await SendCredentials();
            this.Enabled = true;
        }

        private void AddCredentialBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            AddCredential();
            this.Enabled = true;
        }

        private async Task Init()
        {
            try
            {
                var keyValuePairs = new Dictionary<string, string>() { };
                keyValuePairs["api"] = "GetIp";
                keyValuePairs["Token"] = "cf870b1832e928369b7872dd741906e4";

                var result = await utility.CallApi(HttpMethod.Get, "http://supp.altervista.org/", "Config.php", keyValuePairs, null);
                var content = await result.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content);

                var ip = obj["Data"][0]["IP"].ToString();

                var message = obj["Message"];

                AddressTxt.Text = "http://" + ip + ":82";

                //for test
                AddressTxt.Text = "http://localhost:52110";
                GoogleDriveAccountTxt.Text = "daniele.marassi.media1";
                UserNameTxt.Text = "Admin";
                PasswordTxt.Text = "Password!123";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void GoogleDriveCreateCredentialsForm_Load(object sender, EventArgs e)
        {
            await Init();
        }
    }
}
