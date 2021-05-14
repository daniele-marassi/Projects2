using Supp.Site.Common;
using Supp.Site.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;
using Additional.NLog;
using Additional;
using GoogleDriveManagerModels;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Supp.Site.Repositories
{
    public class MediaRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly SuppUtility suppUtility;
        private readonly Utility utility;

        public MediaRepository()
        {
            suppUtility = new SuppUtility();
            utility = new Utility();
        }

        /// <summary>
        /// Get All Media
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> GetAllMedia(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };


                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Media/GetAllMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get Media By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> GetMediaById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Media/GetMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Update Media
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> UpdateMedia(MediaDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null)
                            keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }



                    var result = await utility.CallApi(HttpMethod.Put, GeneralSettings.Static.BaseUrl, "api/Media/UpdateMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add Media
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> AddMedia(MediaDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null)
                            keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }



                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/Media/AddMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add Range Media
        /// </summary>
        /// <param name="dataJsonString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> AddRangeMedia(string dataJsonString, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs[""] = dataJsonString;

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/Media/AddRangeMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete Media By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MediaResult> DeleteMediaById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/Media/DeleteMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<MediaResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get Structure Media
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ManagerResult> GetStructureMedia(string token, string userName, long userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ManagerResult() { Data = new List<RequestResult>(), ResultState = new GoogleDriveManagerModels.ResultType()};

                try
                {
                    var requests = new List<ManagerRequest>() { };
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var identityMD5 = String.Empty;

                    using (MD5 md5Hash = MD5.Create())
                    {
                        identityMD5 = Common.SuppUtility.GetMd5Hash(md5Hash, identity);
                    }
                   
                    var googleDriveAccountRepository = new GoogleDriveAccountsRepository() { };
                    var googleDriveAuthsRepository = new GoogleDriveAuthsRepository() { };
                    var mediaConfigurationsRepository = new MediaConfigurationsRepository() { };

                    var googleDriveAccountResult = await googleDriveAccountRepository.GetAllGoogleDriveAccounts(token);
                    var googleDriveAuthResult = await googleDriveAuthsRepository.GetAllGoogleDriveAuths(token);
                    var mediaConfigurationResult = await mediaConfigurationsRepository.GetAllMediaConfigurations(token);

                    var googleDriveAccounts = googleDriveAccountResult.Data.Where(_ => _.UserId == userId).ToList();
                    var googleDriveAuthIds = googleDriveAccounts.Select(_ => _.GoogleDriveAuthId).ToList();
                    var mediaConfiguration = mediaConfigurationResult.Data.Where(_ => _.UserId == userId).FirstOrDefault();

                    foreach (var account in googleDriveAccounts)
                    {
                        var auth = googleDriveAuthResult.Data.Where(_ => _.Id == account.GoogleDriveAuthId).FirstOrDefault();
                        requests = new List<ManagerRequest>() 
                        { 
                            new ManagerRequest()
                            {
                                Auth = new Auth(){ 
                                    Installed = new AuthProperties()
                                    {
                                        Client_id = auth.Client_id,
                                        Client_secret = auth.Client_secret,
                                        Project_id = auth.Project_id           
                                    } 
                                },
                                GoogleDriveAccountId = account.GoogleDriveAuthId,
                                Account = account.Account, 
                                Action = ActionType.GetStructure, 
                                FolderToFilter = account.FolderToFilter, 
                                FileName = null, 
                                FileId = null, 
                                MaxThumbnailSize = mediaConfiguration.MaxThumbnailSize, 
                                MinThumbnailSize = mediaConfiguration.MinThumbnailSize
                            }
                        };
                    }

                    /////////////////////////////////////////////////////////////////////////////TEST
                    requests.FirstOrDefault().FolderToFilter = "Roberta_me";

                    var googleDriveManagerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GoogleDriveManager");

                    var inputPath = Path.Combine(googleDriveManagerPath, "Input");
                    if (!Directory.Exists(inputPath)) Directory.CreateDirectory(inputPath);
                    System.IO.File.WriteAllText(Path.Combine(inputPath, $"RequestList_{identityMD5}.json"), JsonConvert.SerializeObject(requests));

                    var command = identityMD5;
                    var cmdsi = new ProcessStartInfo(Path.Combine(googleDriveManagerPath, "GoogleDriveManager.exe"));
                    cmdsi.Arguments = command;
                    var cmd = Process.Start(cmdsi);
                    cmd.WaitForExit();

                    var result = File.ReadAllText(Path.Combine(googleDriveManagerPath, "Output", $"Result_{identityMD5}.json"));

                    try
                    {
                        //File.Delete(Path.Combine(inputPath, $"GoogleDriveRequestList_{identityMD5}.json"));
                        //File.Delete(Path.Combine(googleDriveManagerPath, "Output", $"Result_{identityMD5}.json"));
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }

                    var managerResult = JsonConvert.DeserializeObject<ManagerResult>(result);

                    response = managerResult;
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = GoogleDriveManagerModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }
    }
}