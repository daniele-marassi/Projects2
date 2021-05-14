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
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Models.Entities.Automation;

namespace Mair.DigitalSuite.ServiceHost.Repositories
{
    public class TimersRepository : ITimersRepository, IDisposable
    {
        private MairDigitalSuiteDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public TimersRepository(MairDigitalSuiteDatabaseContext context)
        {
            db = context;
        }

        private bool TimerExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Timers.Count(_ => _.Id == id) > 0;
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
        /// Get All Timers
        /// </summary>
        /// <returns></returns>
        public async Task<TimerResult> GetAllTimers()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var timers = await db.Timers.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Timer, TimerDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<TimerDto>>(timers);

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
        /// Get Timers By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TimerResult> GetTimersById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var timer = await db.Timers.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Timer, TimerDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<TimerDto>(timer);

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
        /// Update Timer
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public async Task<TimerResult> UpdateTimer(TimerDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TimerDto, Timer>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Timer>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!TimerExists(dto.Id))
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
        /// Add Timer
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public async Task<TimerResult> AddTimer(TimerDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TimerDto, Timer>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Timer>(dto);

                    db.Timers.Add(data);
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
        /// Delete Timer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TimerResult> DeleteTimerById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var timer = await db.Timers.FindAsync(id);
                    if (timer == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Timers.Remove(timer);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Timer, TimerDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<TimerDto>(timer);

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
