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
    public class TagsRepository : ITagsRepository, IDisposable
    {
        private MairDigitalSuiteDatabaseContext db;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public TagsRepository(MairDigitalSuiteDatabaseContext context)
        {
            db = context;
        }

        private bool TagExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool exists = false;
                try
                {
                    exists = db.Tags.Count(_ => _.Id == id) > 0;
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
        /// Get All Tags
        /// </summary>
        /// <returns></returns>
        public async Task<TagResult> GetAllTags()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var tags = await db.Tags.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<TagDto>>(tags);

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
        /// Get Tags By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TagResult> GetTagsById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var tag = await db.Tags.Where(_ => _.Id == id).FirstOrDefaultAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<TagDto>(tag);

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
        /// Update Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<TagResult> UpdateTag(TagDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TagDto, Tag>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Tag>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }
                catch (Exception ex)
                {
                    if (!TagExists(dto.Id))
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
        /// Add Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<TagResult> AddTag(TagDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TagDto, Tag>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<Tag>(dto);

                    db.Tags.Add(data);
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
        /// Delete Tag By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TagResult> DeleteTagById(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var tag = await db.Tags.FindAsync(id);
                    if (tag == null)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "";
                    }
                    else
                    {
                        db.Tags.Remove(tag);
                        await db.SaveChangesAsync();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<TagDto>(tag);

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
