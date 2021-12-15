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
    public class UserRoleTypesRepository : IUserRoleTypesRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public UserRoleTypesRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool UserRoleTypeExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.UserRoleTypes.Count(_ => _.Id == id) > 0;
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
        /// Get All UserRoleTypes
        /// </summary>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> GetAllUserRoleTypes()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRoleTypes = await db.UserRoleTypes.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleType, UserRoleTypeDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<UserRoleTypeDto>>(userRoleTypes);

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
        /// Get UserRoleTypes By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> GetUserRoleTypesById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRoleType = await db.UserRoleTypes.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleType, UserRoleTypeDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<UserRoleTypeDto>(userRoleType);

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
        /// Update UserRoleType
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> UpdateUserRoleType(UserRoleTypeDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleTypeDto, UserRoleType>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<UserRoleType>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!UserRoleTypeExists(dto.Id))
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
        /// Add UserRoleType
        /// </summary>
        /// <param name="userRoleType"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> AddUserRoleType(UserRoleTypeDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleTypeDto, UserRoleType>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<UserRoleType>(dto);

                    db.UserRoleTypes.Add(data);
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
        /// Delete UserRoleType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> DeleteUserRoleTypeById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRoleType = await db.UserRoleTypes.FindAsync(id);
                    if (userRoleType == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.UserRoleTypes.Remove(userRoleType);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleType, UserRoleTypeDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<UserRoleTypeDto>(userRoleType);

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
