using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Repositories;
using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.TagDispatcher.Servicies;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Mair.DigitalSuite.ServiceHost.Common.Config.GeneralSettings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Models.Dto;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;

namespace Mair.DigitalSuite.ServiceHost.Services.Plc
{
    public class PlcManager
    {
        private PlcConnector _plcConnector;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private IEventsRepository iEventsRepo;
        private INodesRepository iNodesRepo;
        private ITagsRepository iTagsRepo;
        private ITimersRepository iTimersRepo;
        private MairDigitalSuiteDatabaseContext _context;

        public PlcManager(MairDigitalSuiteDatabaseContext context)
        {
            _plcConnector = new PlcConnector(Static.MairDigitalSuiteDatabaseConnection);
            _context = context;
            iEventsRepo = new EventsRepository(context);
            iNodesRepo = new NodesRepository(context);
            iTagsRepo = new TagsRepository(context);
            iTimersRepo = new TimersRepository(context);
        }

        public async Task<PlcDataResult> GetPlcData()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };
                try
                {
                    List<TagDispatcher.Models.Dto.Automation.PlcDataDto> data = new List<TagDispatcher.Models.Dto.Automation.PlcDataDto>() { };
                    var events = iEventsRepo.GetAllEvents().Result.Data.ToList();

                    foreach (var _event in events)
                    {
                        data = new List<TagDispatcher.Models.Dto.Automation.PlcDataDto>() { };

                        var tagStart = iTagsRepo.GetTagsById(_event.PlcStartId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeStart = iNodesRepo.GetNodesById(tagStart.NodeId).Result.Data.FirstOrDefault();
                        var resultStart = _plcConnector.GetPlcData(nodeStart.Driver, nodeStart.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataStart = resultStart.Data.Where(_ => _.TagAddress == tagStart.Address).FirstOrDefault();

                        data.Add(plcDataStart);

                        var tagEnd = iTagsRepo.GetTagsById(_event.PlcEndId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeEnd = iNodesRepo.GetNodesById(tagEnd.NodeId).Result.Data.FirstOrDefault();
                        var resultEnd = _plcConnector.GetPlcData(nodeEnd.Driver, nodeEnd.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataEnd = resultEnd.Data.Where(_ => _.TagAddress == tagEnd.Address).FirstOrDefault();

                        data.Add(plcDataEnd);

                        var tagAck = iTagsRepo.GetTagsById(_event.PlcAckId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeAck = iNodesRepo.GetNodesById(tagAck.NodeId).Result.Data.FirstOrDefault();
                        var resultAck = _plcConnector.GetPlcData(nodeAck.Driver, nodeAck.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataAck = resultAck.Data.Where(_ => _.TagAddress == tagAck.Address).FirstOrDefault();

                        data.Add(plcDataAck);

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<TagDispatcher.Models.Dto.Automation.PlcDataDto, PlcDataDto>());
                        var mapper = config.CreateMapper();
                        var dto = mapper.Map<List<PlcDataDto>>(data);

                        response.Data.AddRange(dto);
                    }

                    if (response.Data.Count == 0)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "Events not found!";
                    }
                    else
                    {
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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        public async Task<DashboardDataResult> GetDashboardData()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new DashboardDataResult() { Data = new List<DashboardDataDto>(), ResultState = new ResultType() };
                try
                {
                    var dto = new List<DashboardDataDto>() { };
                    var events = iEventsRepo.GetAllEvents().Result.Data.ToList();

                    foreach (var _event in events)
                    {
                        dto = new List<DashboardDataDto>() { };

                        var timer = iTimersRepo.GetTimersById(_event.TimerId).Result.Data.FirstOrDefault();

                        var tagStart = iTagsRepo.GetTagsById(_event.PlcStartId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeStart = iNodesRepo.GetNodesById(tagStart.NodeId).Result.Data.FirstOrDefault();
                        var resultStart = _plcConnector.GetPlcData(nodeStart.Driver, nodeStart.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataStart = resultStart.Data.Where(_ => _.TagAddress == tagStart.Address).FirstOrDefault();

                        dto.Add(new DashboardDataDto()
                        {
                            EventName = _event.Name,
                            EventDescription = _event.Description,
                            EventType = _event.Type,
                            TimerName = timer.Name,
                            TimerInterval = timer.Inteval,
                            NodeName = nodeStart.Name,
                            NodeDescription = nodeStart.Description,
                            PlcConnectionString = nodeStart.ConnectionString,
                            PlcDriver = nodeStart.Driver,
                            TagName = tagStart.Name,
                            TagDescription = tagStart.Description,
                            TagValue = plcDataStart.TagValue
                        });

                        var tagEnd = iTagsRepo.GetTagsById(_event.PlcEndId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeEnd = iNodesRepo.GetNodesById(tagEnd.NodeId).Result.Data.FirstOrDefault();
                        var resultEnd = _plcConnector.GetPlcData(nodeEnd.Driver, nodeEnd.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataEnd = resultEnd.Data.Where(_ => _.TagAddress == tagEnd.Address).FirstOrDefault();

                        dto.Add(new DashboardDataDto()
                        {
                            EventName = _event.Name,
                            EventDescription = _event.Description,
                            EventType = _event.Type,
                            TimerName = timer.Name,
                            TimerInterval = timer.Inteval,
                            NodeName = nodeEnd.Name,
                            NodeDescription = nodeEnd.Description,
                            PlcConnectionString = nodeEnd.ConnectionString,
                            PlcDriver = nodeEnd.Driver,
                            TagName = tagEnd.Name,
                            TagDescription = tagEnd.Description,
                            TagValue = plcDataEnd.TagValue
                        });

                        var tagAck = iTagsRepo.GetTagsById(_event.PlcAckId).Result.Data.Where(_ => _.Enable == true).FirstOrDefault();
                        var nodeAck = iNodesRepo.GetNodesById(tagAck.NodeId).Result.Data.FirstOrDefault();
                        var resultAck = _plcConnector.GetPlcData(nodeAck.Driver, nodeAck.ConnectionString, Config.GeneralSettings.Static.Emulation);
                        var plcDataAck = resultAck.Data.Where(_ => _.TagAddress == tagAck.Address).FirstOrDefault();

                        dto.Add(new DashboardDataDto()
                        {
                            EventName = _event.Name,
                            EventDescription = _event.Description,
                            EventType = _event.Type,
                            TimerName = timer.Name,
                            TimerInterval = timer.Inteval,
                            NodeName = nodeAck.Name,
                            NodeDescription = nodeAck.Description,
                            PlcConnectionString = nodeAck.ConnectionString,
                            PlcDriver = nodeAck.Driver,
                            TagName = tagAck.Name,
                            TagDescription = tagAck.Description,
                            TagValue = plcDataAck.TagValue
                        });

                        response.Data.AddRange(dto);
                    }

                    if (response.Data.Count == 0)
                    {
                        response.Successful = true;
                        response.ResultState = ResultType.NotFound;
                        response.Message = "Events not found!";
                    }
                    else
                    {
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

        public async Task<PlcDataResult> UpdatePlcData(string driver, string connectionString, string tagAddress, string tagValue)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };
                try
                {
                    var result = _plcConnector.UpdatePlcData(driver, connectionString, tagAddress, tagValue, Config.GeneralSettings.Static.Emulation);

                    var data = result.Data.FirstOrDefault();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TagDispatcher.Models.Dto.Automation.PlcDataDto, PlcDataDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<PlcDataDto>(data);

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
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
