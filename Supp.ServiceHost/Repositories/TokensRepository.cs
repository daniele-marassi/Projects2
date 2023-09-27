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
    public class TokensRepository : ITokensRepository, IDisposable
    {
        private SuppDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public TokensRepository(SuppDatabaseContext context)
        {
            db = context;
        }

        private bool TokenExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Tokens.Count(_ => _.Id == id) > 0;
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
        /// Get All Tokens
        /// </summary>
        /// <returns></returns>
        public async Task<TokenResult> GetAllTokens()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var tokens = await db.Tokens.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Token, TokenDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<TokenDto>>(tokens);

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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get Tokens By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TokenResult> GetTokensById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var token = await db.Tokens.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Token, TokenDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<TokenDto>(token);

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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Get Tokens By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TokenResult> GetTokensByUserId(long userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var token = await db.Tokens.Where(_ => _.UserId == userId).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Token, TokenDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<TokenDto>(token);

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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Update Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TokenResult> UpdateToken(TokenDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TokenDto, Token>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Token>(dto);

                    data.InsDateTime = DateTime.Parse(data.InsDateTime.ToString());
                    if (data.ConfigInJson == null) data.ConfigInJson = "";

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!TokenExists(dto.Id))
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                        response.OriginalException = null;
                    }
                    else
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                        response.OriginalException = null;
                        logger.Error(ex.ToString());
                        //throw ex;
                    }
                }
                return response;
            }
        }

        /// <summary>
        /// Add Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TokenResult> AddToken(TokenDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TokenDto, Token>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Token>(dto);

                    data.InsDateTime = DateTime.Now;
                    if (data.ConfigInJson == null) data.ConfigInJson = "";

                    try
                    {
                        db.Tokens.Add(data);

                        await db.SaveChangesAsync();

                        dto.Id = data.Id;
                    }
                    catch (Exception)
                    {

                    }

                    response.Successful = true;
                    response.ResultState = ResultType.Created;
                    response.Message = "";
                    response.Data.Add(dto);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        public async Task<TokenResult> CleanAndAddToken(TokenDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TokenDto, Token>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Token>(dto);

                    data.InsDateTime = DateTime.Now;
                    if(data.ConfigInJson == null) data.ConfigInJson = "";

                    string error = "";

                    var tokens = await db.Tokens?.Where(_ => _.UserId == dto.UserId)?.ToListAsync();

                    if (tokens != null)
                        db.Tokens.RemoveRange(tokens);

                    db.Tokens.Add(data);

                    try
                    {
                        await db.SaveChangesAsync();

                        dto.Id = data.Id;
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        logger.Warn(ex.ToString());
                    }

                    response.Successful = true;
                    response.ResultState = ResultType.Created;
                    response.Message = error;
                    response.Data.Add(dto);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete Token By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TokenResult> DeleteTokenById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var token = await db.Tokens.FindAsync(id);
                    if (token == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Tokens.Remove(token);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Token, TokenDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<TokenDto>(token);

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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete All Tokens By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TokenResult> DeleteAllTokensByUserId(long userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var tokens = await db.Tokens?.Where(_ => _.UserId == userId)?.ToListAsync();
                    if (tokens == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Tokens.RemoveRange(tokens);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Token, TokenDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<List<TokenDto>>(tokens);

                        response.Successful = true;
                        response.ResultState = ResultType.Deleted;
                        response.Message = "";
                        response.Data = dto;
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }
    }
}
