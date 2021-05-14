using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Dto.Automation;
using Mair.DS.Client.Web.Models.Results;
using Mair.DS.Client.Web.Models.Results.Auth;
using Mair.DS.Client.Web.Models.Results.Automation;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Repositories.Automation
{
    public class TagsRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public TagsRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All Tags
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> GetAllTags(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tags", null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.AddRange(JsonConvert.DeserializeObject<List<TagDto>>(content));
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
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> GetTagsById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                     keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tags", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TagDto>(content));
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
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> UpdateTag(TagDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Put, Defaults.BaseUrl, "api/tags", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TagDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Updated;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add Tag
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> AddTag(TagDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Post, Defaults.BaseUrl, "api/tags", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TagDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Created;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
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
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> DeleteTagById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Delete, Defaults.BaseUrl, "api/tags", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TagDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Deleted;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get Tags Paged
        /// </summary>
        /// <param name="fromRecord"></param>
        /// <param name="numberOfrecords"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TagResult> GetTagsPaged(int fromRecord, int numberOfrecords, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TagResult() { Data = new List<TagDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["fromRecord"] = fromRecord.ToString();
                    keyValuePairs["numberOfrecords"] = numberOfrecords.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tags/getTagsPaged", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TagResult>(content);
                        response.Data.AddRange(JsonConvert.DeserializeObject<List<TagDto>>(content));
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
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get Tags Count
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<int> GetTagsCount(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var count = 0;

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["key"] = "";

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tags/getTagsCount", null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == true)
                    {
                        count = int.Parse(content.ToString());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return count;
            }
        }
    }
}