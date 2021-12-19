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

namespace Supp.ServiceHost.Repositories
{
    public class MediaConfigurationsRepository : IMediaConfigurationsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public MediaConfigurationsRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool MediaConfigurationExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.MediaConfigurations.Count(_ => _.Id == id) > 0;
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
        /// Get All MediaConfigurations
        /// </summary>
        /// <returns></returns>
        public async Task<MediaConfigurationResult> GetAllMediaConfigurations()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaConfigurationResult() { Data = new List<MediaConfigurationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var mediaConfigurations = await db.MediaConfigurations.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaConfiguration, MediaConfigurationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<MediaConfigurationDto>>(mediaConfigurations);

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
        /// Get MediaConfigurations By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MediaConfigurationResult> GetMediaConfigurationsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaConfigurationResult() { Data = new List<MediaConfigurationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var mediaConfiguration = await db.MediaConfigurations.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaConfiguration, MediaConfigurationDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<MediaConfigurationDto>(mediaConfiguration);

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
        /// Update MediaConfiguration
        /// </summary>
        /// <param name="mediaConfiguration"></param>
        /// <returns></returns>
        public async Task<MediaConfigurationResult> UpdateMediaConfiguration(MediaConfigurationDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaConfigurationResult() { Data = new List<MediaConfigurationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaConfigurationDto, MediaConfiguration>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<MediaConfiguration>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!MediaConfigurationExists(dto.Id))
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
        /// Add MediaConfiguration
        /// </summary>
        /// <param name="mediaConfiguration"></param>
        /// <returns></returns>
        public async Task<MediaConfigurationResult> AddMediaConfiguration(MediaConfigurationDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaConfigurationResult() { Data = new List<MediaConfigurationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaConfigurationDto, MediaConfiguration>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<MediaConfiguration>(dto);

                    db.MediaConfigurations.Add(data);
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
        /// Delete MediaConfiguration By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MediaConfigurationResult> DeleteMediaConfigurationById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaConfigurationResult() { Data = new List<MediaConfigurationDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var mediaConfiguration = await db.MediaConfigurations.FindAsync(id);
                    if (mediaConfiguration == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.MediaConfigurations.Remove(mediaConfiguration);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaConfiguration, MediaConfigurationDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<MediaConfigurationDto>(mediaConfiguration);

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
