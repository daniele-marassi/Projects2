using AutoMapper;
using Supp.ServiceHost.Common;
using Supp.ServiceHost.Repositories;
using Supp.ServiceHost.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using Additional.NLog;
using GoogleDriveManagerModels;
using Newtonsoft.Json;
using System.IO;

namespace Supp.ServiceHost.Repositories
{
    public class GoogleDriveAccountsRepository : IGoogleDriveAccountsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private IGoogleDriveAuthsRepository iGoogleDriveAuthRepo;
        private IMediaConfigurationsRepository iMediaConfigurationRepo;

        public GoogleDriveAccountsRepository(SuppDatabaseContext context)
        {
            db = context;
            iGoogleDriveAuthRepo = new GoogleDriveAuthsRepository(context);
            iMediaConfigurationRepo = new MediaConfigurationsRepository(context);
        }

        private bool GoogleDriveAccountExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.GoogleDriveAccounts.Count(_ => _.Id == id) > 0;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return exists;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        /// <summary>
        /// Get All GoogleDriveAccounts
        /// </summary>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> GetAllGoogleDriveAccounts()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var googleDriveAccounts = await db.GoogleDriveAccounts.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAccount, GoogleDriveAccountDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<GoogleDriveAccountDto>>(googleDriveAccounts);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = Models.ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get GoogleDriveAccounts By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> GetGoogleDriveAccountsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var googleDriveAccount = await db.GoogleDriveAccounts.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAccount, GoogleDriveAccountDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<GoogleDriveAccountDto>(googleDriveAccount);

                    if (dto != null)
                    {
                        response.Data.Add(dto);
                        response.Successful = true;
                        response.ResultState = Models.ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Update GoogleDriveAccount
        /// </summary>
        /// <param name="googleDriveAccount"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> UpdateGoogleDriveAccount(GoogleDriveAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAccountDto, GoogleDriveAccount>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleDriveAccount>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = Models.ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!GoogleDriveAccountExists(dto.Id))
                    {
                        response.Successful = true;
                        response.ResultState = Models.ResultType.NotFound;
                        response.Message = ex.Message;
                        response.OriginalException = null;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = Models.ResultType.Error;
                        response.Message = ex.Message;
                        response.OriginalException = null;
                        logger.Error(ex.ToString());
                        //throw ex;
                    }
                }
                return response;
            }
        }

        /// <summary>
        /// Add GoogleDriveAccount
        /// </summary>
        /// <param name="googleDriveAccount"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> AddGoogleDriveAccount(GoogleDriveAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>(), ResultState = new Models.ResultType() };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAccountDto, GoogleDriveAccount>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleDriveAccount>(dto);

                    db.GoogleDriveAccounts.Add(data);
                    await db.SaveChangesAsync();

                    dto.Id = data.Id;

                    response.Successful = true;
                    response.ResultState = Models.ResultType.Created;
                    response.Message = "";
                    response.Data.Add(dto);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete GoogleDriveAccount By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> DeleteGoogleDriveAccountById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>(), ResultState = new Models.ResultType() };

                try
                {
                    var googleDriveAccount = await db.GoogleDriveAccounts.FindAsync(id);
                    if (googleDriveAccount == null)
                    {
                        response.Successful = true;
                        response.ResultState = Models.ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.GoogleDriveAccounts.Remove(googleDriveAccount);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAccount, GoogleDriveAccountDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<GoogleDriveAccountDto>(googleDriveAccount);

                        response.Successful = true;
                        response.ResultState = Models.ResultType.Deleted;
                        response.Message = "";
                        response.Data.Add(dto);
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add GoogleDrive Credentials
        /// </summary>
        /// <param name="parametersJsonString"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAccountResult> AddGoogleDriveCredentials(string parametersJsonString)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAccountResult() { ResultState = new Models.ResultType() };

                try
                {
                    var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var resourcesPath = Path.Combine(appPath, "Resources");

                    var credentialsParam = new CredentialsParam() { };
                    credentialsParam = JsonConvert.DeserializeObject<CredentialsParam>(parametersJsonString);

                    if (credentialsParam.Data.Count > 0)
                    {
                        var userId = credentialsParam.Data.FirstOrDefault().UserId;
                        var getMediaConfigurationResult = await iMediaConfigurationRepo.GetAllMediaConfigurations();

                        if (!getMediaConfigurationResult.Successful)
                            throw new Exception(getMediaConfigurationResult.Message);

                        if (getMediaConfigurationResult.Data.Where(_ => _.UserId == userId).Count() == 0)
                        {
                            var mediaConfigurationDto = new MediaConfigurationDto() { UserId = userId, MaxThumbnailSize = 200, MinThumbnailSize = 50 };
                            var addMediaConfigurationResult = await iMediaConfigurationRepo.AddMediaConfiguration(mediaConfigurationDto);

                            if (!addMediaConfigurationResult.Successful)
                                throw new Exception(addMediaConfigurationResult.Message);
                        }

                        foreach (var credential in credentialsParam.Data)
                        {
                            var getGoogleDriveAccountsResult = await GetAllGoogleDriveAccounts();

                            if (!getGoogleDriveAccountsResult.Successful)
                                throw new Exception(getGoogleDriveAccountsResult.Message);

                            var accounts = getGoogleDriveAccountsResult.Data.Where(_ => _.UserId == credential.UserId && _.Account == credential.Account && _.FolderToFilter == credential.FolderToFilter).ToList();
                            foreach (var account in accounts)
                            {
                                var delGoogleDriveAccountResult = await DeleteGoogleDriveAccountById(account.Id);

                                if (!delGoogleDriveAccountResult.Successful)
                                    throw new Exception(delGoogleDriveAccountResult.Message);

                                var delGoogleDriveAuthResult = await iGoogleDriveAuthRepo.DeleteGoogleDriveAuthById(account.GoogleDriveAuthId);

                                if (!delGoogleDriveAuthResult.Successful)
                                    throw new Exception(delGoogleDriveAuthResult.Message);

                            }

                            var googleDriveAuthDto = new GoogleDriveAuthDto() { Client_id = credential.Auth.Installed.Client_id, Client_secret = credential.Auth.Installed.Client_secret, Project_id = credential.Auth.Installed.Project_id };
                            var addGoogleDriveAuthResult = await iGoogleDriveAuthRepo.AddGoogleDriveAuth(googleDriveAuthDto);

                            if (!addGoogleDriveAuthResult.Successful)
                                throw new Exception(addGoogleDriveAuthResult.Message);

                            var googleDriveAccountDto = new GoogleDriveAccountDto() { Account = credential.Account, FolderToFilter = credential.FolderToFilter, GoogleDriveAuthId = addGoogleDriveAuthResult.Data.FirstOrDefault().Id, UserId = credential.UserId };
                            var addGoogleDriveAccountResult = await AddGoogleDriveAccount(googleDriveAccountDto);

                            if (!addGoogleDriveAccountResult.Successful)
                                throw new Exception(addGoogleDriveAccountResult.Message);

                            var apiPath = Path.Combine(resourcesPath, @".credentials\apiName");
                            var tokenResponseFullPath = Path.Combine(apiPath, credential.AccessFileName);
                            if (!Directory.Exists(apiPath)) Directory.CreateDirectory(apiPath);

                            if (System.IO.File.Exists(tokenResponseFullPath)) System.IO.File.Delete(tokenResponseFullPath);

                            System.IO.File.WriteAllText(tokenResponseFullPath, JsonConvert.SerializeObject(credential.AccessProperties));

                            response.Successful = true;
                            response.ResultState = Models.ResultType.Created;
                            response.Message = "Credentials created!";
                        }
                    }
                    else 
                    {
                        response.Successful = true;
                        response.ResultState = Models.ResultType.Failed;
                        response.Message = "ERROR: No Credential sended!";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Models.ResultType.Error;
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
