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
    public class EventsRepository : IEventsRepository, IDisposable
    {
        private MairDigitalSuiteDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public EventsRepository(MairDigitalSuiteDatabaseContext context)
        {
            db = context;
        }

        private bool EventExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Events.Count(_ => _.Id == id) > 0;
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
        /// Get All Events
        /// </summary>
        /// <returns></returns>
        public async Task<EventResult> GetAllEvents()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<EventDto>(), ResultState = new ResultType() };

                try
                {
                    var events = await db.Events.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<EventDto>>(events);

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
        /// Get Events By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EventResult> GetEventsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<EventDto>(), ResultState = new ResultType() };

                try
                {
                    var _event = await db.Events.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<EventDto>(_event);

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
        /// Update Event
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task<EventResult> UpdateEvent(EventDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<EventDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<EventDto, Event>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Event>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!EventExists(dto.Id))
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
        /// Add Event
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task<EventResult> AddEvent(EventDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<EventDto>(), ResultState = new ResultType() };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<EventDto, Event>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Event>(dto);

                    db.Events.Add(data);
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
        /// Delete Event By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EventResult> DeleteEventById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<EventDto>(), ResultState = new ResultType() };

                try
                {
                    var _event = await db.Events.FindAsync(id);
                    if (_event == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Events.Remove(_event);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<EventDto>(_event);

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
