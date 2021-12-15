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
    public class SongsRepository : ISongsRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public SongsRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool SongExists(long id)
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
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var songs = await db.Songs.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Song, SongDto>());
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
                    response.Message = ex.Message;
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
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var song = await db.Songs.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Song, SongDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<SongDto>(song);

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
        /// Update Song
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task<SongResult> UpdateSong(SongDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SongDto, Song>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Song>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!SongExists(dto.Id))
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
        /// Add Song
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task<SongResult> AddSong(SongDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SongDto, Song>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Song>(dto);

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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete Song By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongResult> DeleteSongById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new SongResult() { Data = new List<SongDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var song = await db.Songs.FindAsync(id);
                    if (song == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Songs.Remove(song);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Song, SongDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<SongDto>(song);

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
