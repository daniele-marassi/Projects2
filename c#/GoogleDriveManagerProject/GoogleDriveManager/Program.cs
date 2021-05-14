using Additional;
using Additional.NLog;
using AutoMapper;
using GoogleDriveManagerModels;
using GoogleDriveService;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriveManager
{
    static class Program
    {
        private static Utility utility = new Utility();
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private static NLogUtility nLogUtility = new NLogUtility();

        public static void Execute(string identity)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var managerResult = new ManagerResult() { Message = null, Data = new List<RequestResult>() { }, ResultState = ResultType.None, OriginalException = null, Successful = true};
                var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var errors = new List<string>() { };
                try
                {
                    var googleDriveServiceUtility = new GoogleDriveServiceUtility();
                    var requests = new List<ManagerRequest>() { };
                    var inputPath = Path.Combine(appPath, "Input");
                    if (!Directory.Exists(inputPath)) Directory.CreateDirectory(inputPath);

                    try
                    {
                        var requestListText = File.ReadAllText(Path.Combine(inputPath, $"RequestList_{identity}.json"));
                        requests.AddRange(JsonConvert.DeserializeObject<List<ManagerRequest>>(requestListText));
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }

                    if (requests == null || requests.Count() == 0)
                    {
                        //test
                        //var auth_danielemarassimedia1 = new Auth() { Installed = new AuthProperties() { Client_id = "847493049217-hpbsfk8cco8af0h00cpc1k5ob92crhc7.apps.googleusercontent.com", Project_id = "quickstart-1584214177575", Auth_uri = "https://accounts.google.com/o/oauth2/auth", Token_uri = "https://oauth2.googleapis.com/token", Auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs", Client_secret = "2BJ3Xqt0pBrFABvG0oKgq1MS", Redirect_uris = new string[] { "urn:ietf:wg:oauth:2.0:oob", "http://localhost" } } };
                        //var auth_danielemarassimedia2 = new Auth() { Installed = new AuthProperties() { Client_id = "122870764809-rnh15396144etamd1dr0ghbcafam348s.apps.googleusercontent.com", Project_id = "quickstart-1584533829843", Auth_uri = "https://accounts.google.com/o/oauth2/auth", Token_uri = "https://oauth2.googleapis.com/token", Auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs", Client_secret = "Ok442E_4Rh9hCDPqllq6xaGe", Redirect_uris = new string[] { "urn:ietf:wg:oauth:2.0:oob", "http://localhost" } } };
                        //requests.Add(new ManagerRequest() { Auth = auth_danielemarassimedia1, Account = "daniele.marassi.media1", Action = ActionType.GetStructure, FolderToFilter = "Roberta_me", FileName = null, FileId = null, MaxThumbnailSize = 200, MinThumbnailSize= 50 });
                        //requests.Add(new ManagerRequest() { Auth = auth_danielemarassimedia2, Account = "daniele.marassi.media2", Action = ActionType.GetStructure, FolderToFilter = "", FileName = null, FileId = null, MaxThumbnailSize = 200, MinThumbnailSize = 50 });
                        ////requests.Add(new ManagerRequest() { Auth = auth_danielemarassimedia1, Account = "daniele.marassi.media1", Action = ActionType.GetFile, FolderToFilter = null, FileName = "20200126_131029.jpg", FileId = "1mQjH97cSv9afRh6C1-ReFgRRO1_wsZIK" });

                        if (requests == null || requests.Count() == 0)
                            throw new Exception("No parameter found!");
                    }

                    foreach (var request in requests)
                    {
                        try
                        {
                            var _service = googleDriveServiceUtility.CreateService(request);

                            if (request.Action == ActionType.GetStructure)
                            {
                                managerResult.Data.Add(new RequestResult()
                                {
                                    Files = googleDriveServiceUtility.GetGoogleDriveResult(_service, request, request.FolderToFilter),
                                    Account = request.Account,
                                    Action = request.Action,
                                    Auth = request.Auth
                                });
                            }

                            if (request.Action == ActionType.GetFile)
                            {
                                var stream = googleDriveServiceUtility.DownloadFile(_service, request.FileId);
                                var streamToString = Convert.ToBase64String(stream.ToArray());

                                byte[] data = stream.ToArray();

                                managerResult.Data.Add(new RequestResult()
                                {
                                    Files = new List<FileProperties>() { new FileProperties() { File = data, FileId = request.FileId, Name = request.FileName } },
                                    Account = request.Account,
                                    Action = request.Action,
                                    Auth = request.Auth
                                });

                                //googleDriveService.SaveStream(stream, Path.Combine("c:\\Temp", request.FileName));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.ToString());
                            if (managerResult.Data.Count == 0)
                            {
                                managerResult.Successful = false;
                                managerResult.ResultState = ResultType.Error;
                            }
                            if (managerResult.Data.Count > 0)
                                managerResult.ResultState = ResultType.FoundWithError;
                            errors.Add(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    if (managerResult.Data.Count == 0)
                    {
                        managerResult.Successful = false;
                        managerResult.ResultState = ResultType.Error;
                    }
                    if (managerResult.Data.Count > 0)
                        managerResult.ResultState = ResultType.FoundWithError;
                    errors.Add(ex.Message);
                }

                if (managerResult.Data.Count == 0 && errors.Count == 0)
                    managerResult.ResultState = ResultType.NotFound;
                if (managerResult.Data.Count > 0 && errors.Count == 0)
                    managerResult.ResultState = ResultType.Found;

                if (errors.Count > 0)
                    managerResult.Message = JsonConvert.SerializeObject(errors);

                var outputPath = Path.Combine(appPath, "Output");
                if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
                System.IO.File.WriteAllText(Path.Combine(outputPath, $"Result_{identity}.json"), JsonConvert.SerializeObject(managerResult));
            }
        }

        static void Main(string[] args)
        {
            var identity = String.Empty;
            if (args.Count() > 0) identity = args[0];
            Execute(identity);
        }
    }
}
