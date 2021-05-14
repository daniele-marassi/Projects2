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

namespace Supp.ServiceHost.Repositories
{
    public class GoogleDriveAuthsRepository : IGoogleDriveAuthsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public GoogleDriveAuthsRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool GoogleDriveAuthExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.GoogleDriveAuths.Count(_ => _.Id == id) > 0;
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
        /// Get All GoogleDriveAuths
        /// </summary>
        /// <returns></returns>
        public async Task<GoogleDriveAuthResult> GetAllGoogleDriveAuths()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleDriveAuths = await db.GoogleDriveAuths.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAuth, GoogleDriveAuthDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<GoogleDriveAuthDto>>(googleDriveAuths);

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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get GoogleDriveAuths By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAuthResult> GetGoogleDriveAuthsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleDriveAuth = await db.GoogleDriveAuths.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAuth, GoogleDriveAuthDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<GoogleDriveAuthDto>(googleDriveAuth);

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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Update GoogleDriveAuth
        /// </summary>
        /// <param name="googleDriveAuth"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAuthResult> UpdateGoogleDriveAuth(GoogleDriveAuthDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAuthDto, GoogleDriveAuth>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleDriveAuth>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!GoogleDriveAuthExists(dto.Id))
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = ex.Message;
                        response.OriginalException = null;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
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
        /// Add GoogleDriveAuth
        /// </summary>
        /// <param name="googleDriveAuth"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAuthResult> AddGoogleDriveAuth(GoogleDriveAuthDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAuthDto, GoogleDriveAuth>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<GoogleDriveAuth>(dto);

                    db.GoogleDriveAuths.Add(data);
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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete GoogleDriveAuth By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoogleDriveAuthResult> DeleteGoogleDriveAuthById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var googleDriveAuth = await db.GoogleDriveAuths.FindAsync(id);
                    if (googleDriveAuth == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.GoogleDriveAuths.Remove(googleDriveAuth);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<GoogleDriveAuth, GoogleDriveAuthDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<GoogleDriveAuthDto>(googleDriveAuth);

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
