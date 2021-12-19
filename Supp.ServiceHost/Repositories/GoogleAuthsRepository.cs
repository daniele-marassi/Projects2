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
using System.Text.Json;
using System.Dynamic;

namespace Supp.ServiceHost.Repositories
{
    public class GoogleAuthsRepository : IGoogleAuthsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public GoogleAuthsRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool GoogleAuthExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.GoogleAuths.Count(_ => _.Id == id) > 0;
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
        /// Get All GoogleAuths
        /// </summary>
        /// <returns></returns>
        public async Task<GoogleAuthResult> GetAllGoogleAuths()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAuthResult() { Data = new List<GoogleAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleAuths = await db.GoogleAuths.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAuth, GoogleAuthDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<GoogleAuthDto>>(googleAuths);

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
        /// Get GoogleAuths By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleAuthResult> GetGoogleAuthsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAuthResult() { Data = new List<GoogleAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleAuth = await db.GoogleAuths.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAuth, GoogleAuthDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<GoogleAuthDto>(googleAuth);

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
        /// Update GoogleAuth
        /// </summary>
        /// <param name="googleAuth"></param>
        /// <returns></returns>
        public async Task<GoogleAuthResult> UpdateGoogleAuth(GoogleAuthDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAuthResult() { Data = new List<GoogleAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAuthDto, GoogleAuth>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleAuth>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!GoogleAuthExists(dto.Id))
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
        /// Add GoogleAuth
        /// </summary>
        /// <param name="googleAuth"></param>
        /// <returns></returns>
        public async Task<GoogleAuthResult> AddGoogleAuth(GoogleAuthDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAuthResult() { Data = new List<GoogleAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAuthDto, GoogleAuth>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleAuth>(dto);

                    db.GoogleAuths.Add(data);
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
        /// Delete GoogleAuth By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleAuthResult> DeleteGoogleAuthById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleAuthResult() { Data = new List<GoogleAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleAuth = await db.GoogleAuths.FindAsync(id);
                    if (googleAuth == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.GoogleAuths.Remove(googleAuth);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleAuth, GoogleAuthDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<GoogleAuthDto>(googleAuth);

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
