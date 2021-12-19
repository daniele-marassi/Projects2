using AutoMapper;
using Supp.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using Additional.NLog;

namespace Supp.ServiceHost.Repositories
{
    public class ExecutionQueuesRepository : IExecutionQueuesRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public ExecutionQueuesRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool ExecutionQueueExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.ExecutionQueues.Count(_ => _.Id == id) > 0;
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
        /// Get All ExecutionQueues
        /// </summary>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> GetAllExecutionQueues()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var executionQueues = await db.ExecutionQueues.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Supp.Models.ExecutionQueue, ExecutionQueueDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<ExecutionQueueDto>>(executionQueues);

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
        /// Get ExecutionQueues By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> GetExecutionQueuesById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var executionQueue = await db.ExecutionQueues.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Supp.Models.ExecutionQueue, ExecutionQueueDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<ExecutionQueueDto>(executionQueue);

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
        /// Update ExecutionQueue
        /// </summary>
        /// <param name="executionQueue"></param>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> UpdateExecutionQueue(ExecutionQueueDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ExecutionQueueDto, Supp.Models.ExecutionQueue>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Supp.Models.ExecutionQueue>(dto);

                    data.InsDateTime = DateTime.Parse(data.InsDateTime.ToString());

                    db.Entry(data).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!ExecutionQueueExists(dto.Id))
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
        /// Add ExecutionQueue
        /// </summary>
        /// <param name="executionQueue"></param>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> AddExecutionQueue(ExecutionQueueDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ExecutionQueueDto, Supp.Models.ExecutionQueue>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Supp.Models.ExecutionQueue>(dto);

                    data.InsDateTime = DateTime.Now;

                    db.ExecutionQueues.Add(data);
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
        /// Delete ExecutionQueue By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> DeleteExecutionQueueById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var executionQueue = await db.ExecutionQueues.FindAsync(id);
                    if (executionQueue == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.ExecutionQueues.Remove(executionQueue);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Supp.Models.ExecutionQueue, ExecutionQueueDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<ExecutionQueueDto>(executionQueue);

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
