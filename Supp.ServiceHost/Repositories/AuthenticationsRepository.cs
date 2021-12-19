using AutoMapper;
using Supp.ServiceHost.Common;
using Supp.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using static Supp.ServiceHost.Common.Config;
using System.Diagnostics;
using Additional.NLog;

namespace Supp.ServiceHost.Repositories
{
    public class AuthenticationsRepository : IAuthenticationsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public AuthenticationsRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool AuthenticationExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Authentications.Count(_ => _.Id == id) > 0;
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
        /// Get All Authentications
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAllAuthentications()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var authentications = await db.Authentications.ToListAsync();
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Authentication, AuthenticationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<AuthenticationDto>>(authentications);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get Authentications By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAuthenticationsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var authentication = await db.Authentications.Where(_ => _.Id == id).FirstOrDefaultAsync();
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Authentication, AuthenticationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<AuthenticationDto>(authentication);

                    if (dto != null)
                    {
                        response.Data.Add(dto);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get Authentications By UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAuthenticationsByUserName(string userName)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    List<User> users = null;
                    try
                    {
                        users = await db.Users.ToListAsync();
                    }
                    catch (Exception ex)
                    {
                        if (db == null || db.Users == null)
                        {
                            Process.Start("shutdown", "/r /t 20");
                            throw new Exception("Database access failed! Server restarted, try again in a few minutes!");
                        }
                        else throw ex;  
                    }

                    if (users == null || users.Count() == 0) throw new Exception("Users not found!");

                    var user = users.Where(_=>_.UserName.ToLower().Trim() == userName.ToLower().Trim())?.FirstOrDefault();

                    if (user == null) throw new Exception($"User: [{userName}] not found!");

                    var authentications = await db.Authentications.ToListAsync();

                    if (authentications == null || authentications.Count() == 0) throw new Exception("Authentications not found!");

                    var _authentications = authentications.Where(_ => _.UserId == user.Id)?.ToList();

                    if (_authentications == null) throw new Exception($"Authentications to user: [{userName}] not found!");

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Authentication, AuthenticationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<AuthenticationDto>>(_authentications);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Disable Authentications By UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> DisableAuthenticationsByUserName(string userName)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var users = await db.Users.ToListAsync();

                    if (users == null || users.Count() == 0) throw new Exception("Users not found!");

                    var user = users.Where(_ => _.UserName.ToLower().Trim() == userName.ToLower().Trim())?.FirstOrDefault();

                    if (user == null) throw new Exception($"User: [{userName}] not found!");

                    var authentications = await db.Authentications.ToListAsync();

                    if (authentications == null || authentications.Count() == 0) throw new Exception("Authentications not found!");

                    var _authentications = authentications.Where(_ => _.UserId == user.Id && _.Enable == true)?.ToList();

                    if (_authentications == null) throw new Exception($"Authentications to user: [{userName}] not found!");

                    foreach (var authentication in _authentications)
                    {
                        authentication.Enable = false;
                        db.Entry(authentication).State = EntityState.Modified;
                    }
 
                    await db.SaveChangesAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Authentication, AuthenticationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<AuthenticationDto>>(_authentications);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Update Authentication
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> UpdateAuthentication(AuthenticationDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<AuthenticationDto, Authentication>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Authentication>(dto);

                    db.Entry(dto).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!AuthenticationExists(dto.Id))
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                        response.OriginalException = null;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
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
        /// Add Authentication
        /// </summary>
        /// <param name="authentication"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> AddAuthentication(AuthenticationDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<AuthenticationDto, Authentication>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Authentication>(dto);

                    data.InsDateTime = DateTime.Now;

                    db.Authentications.Add(data);
                    await db.SaveChangesAsync();

                    dto.Id = data.Id;

                    response.Successful = true;
                    response.ResultState = ResultType.Created;
                    response.Message = "";
                    response.Data.Add(dto);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete Authentication By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> DeleteAuthenticationById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var authentication = await db.Authentications.FindAsync(id);
                    if (authentication == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Authentications.Remove(authentication);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Authentication, AuthenticationDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<AuthenticationDto>(authentication);

                        response.Successful = true;
                        response.ResultState = ResultType.Deleted;
                        response.Message = "";
                        response.Data.Add(dto);
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
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
