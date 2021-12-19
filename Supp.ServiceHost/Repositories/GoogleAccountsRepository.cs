using AutoMapper;
using Supp.ServiceHost.Common;
using Supp.ServiceHost.Repositories;
using Supp.Models;
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
using GoogleManagerModels;
using Newtonsoft.Json;
using System.IO;

namespace Supp.ServiceHost.Repositories
{
    public class GoogleAccountsRepository : IGoogleAccountsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private IGoogleAuthsRepository iGoogleAuthRepo;
        private IMediaConfigurationsRepository iMediaConfigurationRepo;

        public GoogleAccountsRepository(SuppDatabaseContext context)
        {
            db = context;
            iGoogleAuthRepo = new GoogleAuthsRepository(context);
            iMediaConfigurationRepo = new MediaConfigurationsRepository(context);
        }

        private bool GoogleAccountExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.GoogleAccounts.Count(_ => _.Id == id) > 0;
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
        /// Get All GoogleAccounts
        /// </summary>
        /// <returns></returns>
        public async Task<GoogleAccountResult> GetAllGoogleAccounts()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { Data = new List<GoogleAccountDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var googleAccounts = await db.GoogleAccounts.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAccount, GoogleAccountDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<GoogleAccountDto>>(googleAccounts);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get GoogleAccounts By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleAccountResult> GetGoogleAccountsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { Data = new List<GoogleAccountDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var googleAccount = await db.GoogleAccounts.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAccount, GoogleAccountDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<GoogleAccountDto>(googleAccount);

                    if (dto != null)
                    {
                        response.Data.Add(dto);
                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Update GoogleAccount
        /// </summary>
        /// <param name="googleAccount"></param>
        /// <returns></returns>
        public async Task<GoogleAccountResult> UpdateGoogleAccount(GoogleAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { Data = new List<GoogleAccountDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAccountDto, GoogleAccount>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleAccount>(dto);

                    data.InsDateTime = DateTime.Parse(data.InsDateTime.ToString());

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = Supp.Models.ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!GoogleAccountExists(dto.Id))
                    {
                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.NotFound;
                        response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                        response.OriginalException = null;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
                        response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                        response.OriginalException = null;
                        logger.Error(ex.ToString());
                        //throw ex;
                    }
                }

                return response;
            }
        }

        /// <summary>
        /// Add GoogleAccount
        /// </summary>
        /// <param name="googleAccount"></param>
        /// <returns></returns>
        public async Task<GoogleAccountResult> AddGoogleAccount(GoogleAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { Data = new List<GoogleAccountDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAccountDto, GoogleAccount>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleAccount>(dto);

                    data.InsDateTime = DateTime.Now;

                    db.GoogleAccounts.Add(data);
                    await db.SaveChangesAsync();

                    dto.Id = data.Id;

                    response.Successful = true;
                    response.ResultState = Supp.Models.ResultType.Created;
                    response.Message = "";
                    response.Data.Add(dto);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Delete GoogleAccount By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleAccountResult> DeleteGoogleAccountById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { Data = new List<GoogleAccountDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var googleAccount = await db.GoogleAccounts.FindAsync(id);
                    if (googleAccount == null)
                    {
                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.GoogleAccounts.Remove(googleAccount);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAccount, GoogleAccountDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<GoogleAccountDto>(googleAccount);

                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.Deleted;
                        response.Message = "";
                        response.Data.Add(dto);
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Add Google Credentials
        /// </summary>
        /// <param name="parametersJsonString"></param>
        /// <returns></returns>
        public async Task<GoogleAccountResult> AddGoogleCredentials(string parametersJsonString)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAccountResult() { ResultState = new Supp.Models.ResultType() };

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
                            var getGoogleAccountsResult = await GetAllGoogleAccounts();

                            if (!getGoogleAccountsResult.Successful)
                                throw new Exception(getGoogleAccountsResult.Message);

                            var accounts = getGoogleAccountsResult.Data.Where(_ => _.UserId == credential.UserId && _.Account == credential.Account && _.FolderToFilter == credential.FolderToFilter && _.AccountType == credential.AccountType).ToList();
                            foreach (var account in accounts)
                            {
                                var delGoogleAccountResult = await DeleteGoogleAccountById(account.Id);

                                if (!delGoogleAccountResult.Successful)
                                    throw new Exception(delGoogleAccountResult.Message);

                                var delGoogleAuthResult = await iGoogleAuthRepo.DeleteGoogleAuthById(account.GoogleAuthId);

                                if (!delGoogleAuthResult.Successful)
                                    throw new Exception(delGoogleAuthResult.Message);
                            }

                            var googleAuthDto = new GoogleAuthDto() { Client_id = credential.Auth.Installed.Client_id, Client_secret = credential.Auth.Installed.Client_secret, Project_id = credential.Auth.Installed.Project_id, TokenFileInJson = credential.TokenFileInJson, GooglePublicKey = credential.GooglePublicKey };
                            var addGoogleAuthResult = await iGoogleAuthRepo.AddGoogleAuth(googleAuthDto);

                            if (!addGoogleAuthResult.Successful)
                                throw new Exception(addGoogleAuthResult.Message);

                            var googleAccountDto = new GoogleAccountDto() { Account = credential.Account, FolderToFilter = credential.FolderToFilter, GoogleAuthId = addGoogleAuthResult.Data.FirstOrDefault().Id, UserId = credential.UserId, AccountType = credential.AccountType };
                            var addGoogleAccountResult = await AddGoogleAccount(googleAccountDto);

                            if (!addGoogleAccountResult.Successful)
                                throw new Exception(addGoogleAccountResult.Message);

                            credential.AccessProperties.Expires_in = 3599;

                            var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                            if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                            var tokenFileName = credential.AccessFileName;
                            var tokenFileFullPath = Path.Combine(tokenFilePath, tokenFileName);

                            if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                            if (System.IO.File.Exists(tokenFileFullPath)) System.IO.File.Delete(tokenFileFullPath);

                            System.IO.File.WriteAllText(tokenFileFullPath, JsonConvert.SerializeObject(credential.AccessProperties));

                            response.Successful = true;
                            response.ResultState = Supp.Models.ResultType.Created;
                            response.Message = "Credentials created!";
                        }
                    }
                    else 
                    {
                        response.Successful = true;
                        response.ResultState = Supp.Models.ResultType.Failed;
                        response.Message = "ERROR: No Credential sended!";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = Supp.Models.ResultType.Error;
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
