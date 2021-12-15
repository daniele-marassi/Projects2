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
    public class WebSpeechesRepository : IWebSpeechesRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public WebSpeechesRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool WebSpeechExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.WebSpeeches.Count(_ => _.Id == id) > 0;
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
        /// Get All WebSpeeches
        /// </summary>
        /// <returns></returns>
        public async Task<WebSpeechResult> GetAllWebSpeeches()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var webSpeeches = await db.WebSpeeches.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<WebSpeech, WebSpeechDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<WebSpeechDto>>(webSpeeches);

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
        /// Get WebSpeeches By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> GetWebSpeechesById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var webSpeech = await db.WebSpeeches.AsNoTracking().Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<WebSpeech, WebSpeechDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<WebSpeechDto>(webSpeech);

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
        /// Update WebSpeech
        /// </summary>
        /// <param name="webSpeech"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> UpdateWebSpeech(WebSpeechDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<WebSpeechDto, WebSpeech>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<WebSpeech>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!WebSpeechExists(dto.Id))
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
        /// Add WebSpeech
        /// </summary>
        /// <param name="webSpeech"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> AddWebSpeech(WebSpeechDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<WebSpeechDto, WebSpeech>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<WebSpeech>(dto);

                    db.WebSpeeches.Add(data);
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
        /// Delete WebSpeech By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> DeleteWebSpeechById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var webSpeech = await db.WebSpeeches.FindAsync(id);
                    if (webSpeech == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.WebSpeeches.Remove(webSpeech);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<WebSpeech, WebSpeechDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<WebSpeechDto>(webSpeech);

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
