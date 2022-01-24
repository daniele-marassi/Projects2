using Supp.Site.Common;
using Supp.Models;
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
using GoogleManagerModels;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Linq;
using GoogleDrive;

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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Media/GetAllMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Media/GetMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

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
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

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
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs[""] = dataJsonString;

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/Media/AddRangeMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/Media/DeleteMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Clear Structure Media
        /// </summary>
        /// <param name="token"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<MediaResult> ClearStructureMedia(string token, string path)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Path"] = path;

                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/Media/ClearStructureMedia", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Structure Media
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RequestResult> StructureMedia(string token, string userName, long userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RequestResult() { Data = new List<Request>(), ResultState = new GoogleManagerModels.ResultType()};

                try
                {
                    var requests = new List<ManagerRequest>() { };
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                   
                    var googleAccountRepository = new GoogleAccountsRepository() { };
                    var googleAuthsRepository = new GoogleAuthsRepository() { };
                    var mediaConfigurationsRepository = new MediaConfigurationsRepository() { };

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(token);
                    var googleAuthResult = await googleAuthsRepository.GetAllGoogleAuths(token);
                    var mediaConfigurationResult = await mediaConfigurationsRepository.GetAllMediaConfigurations(token);

                    var googleAccounts = googleAccountResult.Data.Where(_ => _.UserId == userId && _.AccountType == AccountType.Drive.ToString()).ToList();
                    var googleAuthIds = googleAccounts.Select(_ => _.GoogleAuthId).ToList();
                    var mediaConfiguration = mediaConfigurationResult.Data.Where(_ => _.UserId == userId).FirstOrDefault();

                    foreach (var account in googleAccounts)
                    {
                        var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                        var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);
                        var accessProperties = JsonConvert.DeserializeObject<AccessProperties>(tokenFile.Content);
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
                                GoogleAccountId = account.GoogleAuthId,
                                Account = account.Account, 
                                Action = ActionType.GetStructure, 
                                FolderToFilter = account.FolderToFilter, 
                                FileName = null, 
                                FileId = null, 
                                MaxThumbnailSize = mediaConfiguration.MaxThumbnailSize, 
                                MinThumbnailSize = mediaConfiguration.MinThumbnailSize,
                                TokenFileInJson = auth.TokenFileInJson,
                                GooglePublicKey = auth.GooglePublicKey,
                                RefreshToken = accessProperties.Refresh_token
                            }
                        };
                    }

                    var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var googleDriveUtility = new GoogleDriveUtility();
                    response = await googleDriveUtility.GetStructure(requests, appPath);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = GoogleManagerModels.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }
    }
}