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
    public class UserRolesRepository : IUserRolesRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public UserRolesRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool UserRoleExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.UserRoles.Count(_ => _.Id == id) > 0;
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
        /// Get All UserRoles
        /// </summary>
        /// <returns></returns>
        public async Task<UserRoleResult> GetAllUserRoles()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleResult() { Data = new List<UserRoleDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRoles = await db.UserRoles.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRole, UserRoleDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<UserRoleDto>>(userRoles);

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
        /// Get UserRoles By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRoleResult> GetUserRolesById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleResult() { Data = new List<UserRoleDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRole = await db.UserRoles.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRole, UserRoleDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<UserRoleDto>(userRole);

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
        /// Update UserRole
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserRoleResult> UpdateUserRole(UserRoleDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleResult() { Data = new List<UserRoleDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleDto, UserRole>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<UserRole>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!UserRoleExists(dto.Id))
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
        /// Add UserRole
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task<UserRoleResult> AddUserRole(UserRoleDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleResult() { Data = new List<UserRoleDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRoleDto, UserRole>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<UserRole>(dto);

                    db.UserRoles.Add(data);
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
        /// Delete UserRole By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRoleResult> DeleteUserRoleById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleResult() { Data = new List<UserRoleDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var userRole = await db.UserRoles.FindAsync(id);
                    if (userRole == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.UserRoles.Remove(userRole);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRole, UserRoleDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<UserRoleDto>(userRole);

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
