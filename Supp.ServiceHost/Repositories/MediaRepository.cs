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
using Newtonsoft.Json;

namespace Supp.ServiceHost.Repositories
{
    public class MediaRepository : IMediaRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public MediaRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool MediaExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Media.Count(_ => _.Id == id) > 0;
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
        /// Get All Media
        /// </summary>
        /// <returns></returns>
        public async Task<MediaResult> GetAllMedia()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var media = await db.Media.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Media, MediaDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<MediaDto>>(media);

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
        /// Get Media By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MediaResult> GetMediaById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var media = await db.Media.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Media, MediaDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<MediaDto>(media);

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
        /// Update Media
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public async Task<MediaResult> UpdateMedia(MediaDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaDto, Media>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Media>(dto);

                    data.InsDateTime = DateTime.Parse(data.InsDateTime.ToString());

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!MediaExists(dto.Id))
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
        /// Add Media
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public async Task<MediaResult> AddMedia(MediaDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaDto, Media>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Media>(dto);

                    data.InsDateTime = DateTime.Now;

                    db.Media.Add(data);
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
        /// Add Range Media
        /// </summary>
        /// <param name="dataJsonString"></param>
        /// <returns></returns>
        public async Task<MediaResult> AddRangeMedia(string dataJsonString)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var dto = JsonConvert.DeserializeObject<List<MediaDto>>(dataJsonString);

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MediaDto, Media>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<List<Media>>(dto);

                    db.Media.AddRange(data);
                    await db.SaveChangesAsync();

                    config = new MapperConfiguration(cfg => cfg.CreateMap<Media, MediaDto>());
                    mapper = config.CreateMapper();
                    dto = mapper.Map<List<MediaDto>>(data);

                    response.Successful = true;
                    response.ResultState = ResultType.Created;
                    response.Message = "";
                    response.Data.AddRange(dto);
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
        /// Delete Media By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MediaResult> DeleteMediaById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MediaResult() { Data = new List<MediaDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var media = await db.Media.FindAsync(id);
                    if (media == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Media.Remove(media);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Media, MediaDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<MediaDto>(media);

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

        /// <summary>
        /// Clear Structure Media
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<SongResult> ClearStructureMedia(string path)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    if(path == null || path == String.Empty) db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[Media]");
                    else db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Media] WHERE [Path] LIKE '%"+ path + "%'");

                    response.Successful = true;
                    response.ResultState = ResultType.Deleted;
                    response.Message = "";

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
