using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Additional.NLog;
using Tools.Songs.Contracts;
using Tools.Songs.Contexts;
using Tools.Songs.Models;
using System.Data.Entity;

namespace Tools.Songs.Repositories
{
    public class SongsRepository : ISongsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public SongsRepository(string connectionString)
        {
            db = new SuppDatabaseContext(connectionString);
        }

        private bool SongsExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Songs.Count(_ => _.Id == id) > 0;
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
        /// Get All Songs
        /// </summary>
        /// <returns></returns>
        public async Task<SongResult> GetAllSongs()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    var songs = await db.Songs.AsNoTracking().ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Songs, SongDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<SongDto>>(songs);

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
        /// Get Songs By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongResult> GetSongsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    var songs = await db.Songs.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Songs, SongDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<SongDto>(songs);

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
        /// Update Songs
        /// </summary>
        /// <param name="songs"></param>
        /// <returns></returns>
        public async Task<SongResult> UpdateSongs(SongDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SongDto, Models.Songs>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Models.Songs>(dto);

                    var existingEntity = db.Songs.Find(dto.Id);
                    db.Entry(existingEntity).CurrentValues.SetValues(dto);
                    db.SaveChanges();

                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!SongsExists(dto.Id))
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
        /// Add Songs
        /// </summary>
        /// <param name="songs"></param>
        /// <returns></returns>
        public async Task<SongResult> AddSongs(SongDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SongDto, Models.Songs>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Models.Songs>(dto);

                    db.Songs.Add(data);
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
        /// Delete Songs By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongResult> DeleteSongsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    var songs = await db.Songs.FindAsync(id);
                    if (songs == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Songs.Remove(songs);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Songs, SongDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<SongDto>(songs);

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
        /// Clear Songs
        /// </summary>
        /// <returns></returns>
        public async Task<SongResult> ClearSongs()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = new ResultType() };

                try
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[Songs]");

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
