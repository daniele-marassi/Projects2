using Additional;
using GoogleCreateCredentials.Models;
using GoogleManagerModels;
using GoogleService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleCreateCredentials
{
    public partial class GoogleCreateCredentialsToolForm : Form
    {
        private static CredentialsParam credentialsParam = new CredentialsParam() { Data = new List<Credential>() { } };
        private static Utility utility = new Utility();
        public GoogleCreateCredentialsToolForm()
        {
            InitializeComponent();
        }

        private void EnableTheGoogleAPIBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://console.cloud.google.com/apis/library");
        }

        private void AddCredential()
        {
            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var resourcesPath = Path.Combine(appPath, "Resources");
            var downloadsPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            downloadsPath = Path.Combine(downloadsPath, "Downloads");
            if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
            if (GoogleAccountTxt.Text == null || GoogleAccountTxt.Text == String.Empty)
            {
                MessageBox.Show("Insert Account");
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = downloadsPath;
                openFileDialog.FileName = "client_secret_.json";
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var account = GoogleAccountTxt.Text.Split('@')[0];

                        var credentialsText = File.ReadAllText(openFileDialog.FileName);

                        var auth = JsonConvert.DeserializeObject<Auth>(credentialsText);
                        var googleServiceUtility = new GoogleServiceUtility();
                        var request = new ManagerRequest() { Auth = auth, Account = account };

                        AccountType accountType = AccountType.Drive;

                        if (GoogleAccountTypeCmb.Text == AccountType.Drive.ToString()) accountType = AccountType.Drive;
                        if (GoogleAccountTypeCmb.Text == AccountType.Calendar.ToString()) accountType = AccountType.Calendar;
                        if (GoogleAccountTypeCmb.Text == AccountType.Note.ToString()) accountType = AccountType.Note;

                        var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                        if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                        var tokenFileName = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-" + account;
                        var tokenFileFullPath = Path.Combine(tokenFilePath, tokenFileName);

                        if(File.Exists(tokenFileFullPath)) File.Delete(tokenFileFullPath);

                        var _service = googleServiceUtility.CreateCredential(request, accountType);

                        var tokenFileInJson = File.ReadAllText(tokenFileFullPath);

                        var tokenFile = new TokenFile() { FileName = tokenFileName, Content = tokenFileInJson }; 

                        var accessProperties = JsonConvert.DeserializeObject<AccessProperties>(tokenFileInJson);

                        credentialsParam.Data.Add( 
                            new Credential() { 
                                Auth = auth, 
                                AccessProperties = accessProperties, 
                                AccessFileName = tokenFileName,
                                Account = account,
                                FolderToFilter = FolderToFilterTxt.Text,
                                AccountType = GoogleAccountTypeCmb.Text,
                                TokenFileInJson = JsonConvert.SerializeObject(tokenFile),
                                GooglePublicKey = GooglePublicKeyTxt.Text
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

                if (credentialsParam.Data.Count == 0)
                {
                    Debug.WriteLine("Credentials is empty!");
                    MessageBox.Show("Credentials is empty!");
                    return;
                }

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

                result = await  utility.CallApi(HttpMethod.Post, AddressTxt.Text, "api/GoogleAccounts/AddGoogleCredentials", keyValuePairs, tokenResult.Data?.FirstOrDefault()?.Token);
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

            if (dirInfo.Exists)
            {
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
            }
            else
            {
                Directory.CreateDirectory(resourcesPath);
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
                var accountTypes = Enum.GetValues(typeof(AccountType));

                foreach (var item in accountTypes)
                {
                    GoogleAccountTypeCmb.Items.Add(item);
                }

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
                }
                catch (Exception)
                {
                }

                GoogleAccountTypeCmb.Text = AccountType.Drive.ToString();

                var value = "";

                try
                {
                    value = ConfigurationManager.AppSettings["GooglePublicKey"];
                    if (value != null && value != String.Empty) GooglePublicKeyTxt.Text = value;

                    value = ConfigurationManager.AppSettings["AccountType"];
                    if(value != null && value != String.Empty) GoogleAccountTypeCmb.Text = value;
                    value = ConfigurationManager.AppSettings["GoogleAccount"];
                    if (value != null && value != String.Empty) GoogleAccountTxt.Text = value;
                    value = ConfigurationManager.AppSettings["FolderToFilter"];
                    if (value != null && value != String.Empty) FolderToFilterTxt.Text = value;
                    value = ConfigurationManager.AppSettings["SuppAddress"];
                    if (value != null && value != String.Empty) AddressTxt.Text = value;
                    value = ConfigurationManager.AppSettings["SuppUserName"];
                    if (value != null && value != String.Empty) UserNameTxt.Text = value;
                    value = ConfigurationManager.AppSettings["SuppPassword"];
                    if (value != null && value != String.Empty) PasswordTxt.Text = value;
                }
                catch (Exception)
                {
                }

                //for test
                GoogleAccountTypeCmb.Text = "Calendar";
                GooglePublicKeyTxt.Text = "AIzaSyCdWVUdy3QmmYLjDwQWqP03gV49hfvWMhc";
                GoogleAccountTxt.Text = "daniele.marassi";
                FolderToFilterTxt.Text = "";
                AddressTxt.Text = "http://localhost:52110";
                UserNameTxt.Text = "daniele.marassi@gmail.com";
                PasswordTxt.Text = "Enilno.00";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void GoogleCreateCredentialsForm_Load(object sender, EventArgs e)
        {
            await Init();
        }
    }
}
