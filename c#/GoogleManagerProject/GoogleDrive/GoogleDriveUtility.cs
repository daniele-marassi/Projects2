using Additional;
using Additional.NLog;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using GoogleManagerModels;
using GoogleService;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive
{
    public class GoogleDriveUtility
    {
        private static Utility utility = new Utility();
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private static NLogUtility nLogUtility = new NLogUtility();

        /// <summary>
        /// Get Structure
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public async Task<RequestResult> GetStructure(List<ManagerRequest> requests, string appPath)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var googleServiceUtility = new GoogleServiceUtility();
                var result = new RequestResult() { Message = null, Data = new List<Request>() { }, ResultState = ResultType.None, OriginalException = null, Successful = true };
                var errors = new List<string>() { };

                foreach (var request in requests)
                {
                    try
                    {
                        var tokenFile = JsonConvert.DeserializeObject<TokenFile>(request.TokenFileInJson);
                        var accessProperties = JsonConvert.DeserializeObject<AccessProperties>(tokenFile.Content);

                        var resourcesPath = Path.Combine(appPath, "Resources");
                        if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
                        var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                        if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                        var tokenFileFullPath = Path.Combine(tokenFilePath, tokenFile.FileName);

                        if (System.IO.File.Exists(tokenFileFullPath)) System.IO.File.Delete(tokenFileFullPath);

                        System.IO.File.WriteAllText(tokenFileFullPath, JsonConvert.SerializeObject(accessProperties));

                        var _service = new DriveService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = googleServiceUtility.CreateCredential(request, AccountType.Drive),
                            ApplicationName = "Google Drive - GetStructure",
                        });

                        if (request.Action == ActionType.GetStructure)
                        {
                            result.Data.Add(new Request()
                            {
                                Files = googleServiceUtility.GetGoogleDriveResult(_service, request, request.FolderToFilter),
                                Account = request.Account,
                                Action = request.Action,
                                Auth = request.Auth
                            });
                        }

                        if (request.Action == ActionType.GetFile)
                        {
                            var stream = googleServiceUtility.DownloadFile(_service, request.FileId);
                            var streamToString = Convert.ToBase64String(stream.ToArray());

                            byte[] data = stream.ToArray();

                            result.Data.Add(new Request()
                            {
                                Files = new List<FileProperties>() { new FileProperties() { File = data, FileId = request.FileId, Name = request.FileName } },
                                Account = request.Account,
                                Action = request.Action,
                                Auth = request.Auth
                            });

                            //googleService.SaveStream(stream, Path.Combine("c:\\Temp", request.FileName));
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());

                        errors.Add(ex.Message);
                    }
                }

                if (result.Data.Count == 0 && errors.Count == 0)
                    result.ResultState = ResultType.NotFound;
                if (result.Data.Count > 0 && errors.Count == 0)
                    result.ResultState = ResultType.Found;
                if (result.Data.Count > 0 && errors.Count > 0)
                    result.ResultState = ResultType.FoundWithError;

                if (result.Data.Count == 0 && errors.Count > 0)
                {
                    result.Successful = false;
                    result.ResultState = ResultType.Error;
                }

                if (errors.Count > 0)
                    result.Message = JsonConvert.SerializeObject(errors);

                return result;
            }
        }
    }
}
