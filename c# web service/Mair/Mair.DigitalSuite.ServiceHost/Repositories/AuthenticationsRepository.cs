using AutoMapper;
using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Models.Result.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Models.Entities.Auth;

namespace Mair.DigitalSuite.ServiceHost.Repositories
{
    public class AuthenticationsRepository : IAuthenticationsRepository, IDisposable
    {
        private MairDigitalSuiteDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public AuthenticationsRepository(MairDigitalSuiteDatabaseContext context)
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

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
                    response.Message = "";
                    response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

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
                    response.Message = "";
                    response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var user = await db.Users.Where(_ => _.UserName == userName).FirstOrDefaultAsync();

                    if (user == null) throw new Exception("User not found!");

                    var authentications = await db.Authentications.Where(_ => _.UserId == user.Id).ToListAsync();
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
                    response.Message = "";
                    response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var user = await db.Users.Where(_ => _.UserName == userName).FirstOrDefaultAsync();

                    if (user == null) throw new Exception("User not found!");

                    var authentications = await db.Authentications.Where(_ => _.UserId == user.Id && _.Enable == true).ToListAsync();

                    foreach (var authentication in authentications)
                    {
                        authentication.Enable = false;
                    }

                    db.Entry(authentications).State = EntityState.Modified;
                    await db.SaveChangesAsync();

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
                    response.Message = "";
                    response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

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
                        response.Message = "";
                        response.OriginalException = ex;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message = "";
                        response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<AuthenticationDto, Authentication>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Authentication>(dto);

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
                    response.Message = "";
                    response.OriginalException = ex;
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
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

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
                    response.Message = "";
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }
    }
}
