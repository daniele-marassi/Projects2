using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Services.Plc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Mair.DigitalSuite.ServiceHost.Common.Config;
using Mair.DigitalSuite.ServiceHost.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mair.DigitalSuite.ServiceHost.Services.Token;
using System.Collections.Generic;
using System.Linq;
using Mair.DigitalSuite.ServiceHost.Models.Result;

namespace Mair.DigitalSuite.ServiceHost.Services.SignalR
{
    public class PlcWorker: BackgroundService
    {
        private readonly IHubContext<PlcHub, IPlcHub> _plcHub;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private MairDigitalSuiteDatabaseContext _context;

        public PlcWorker(IHubContext<PlcHub, IPlcHub> plcHub)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MairDigitalSuiteDatabaseContext>();
            optionsBuilder.UseSqlServer(GeneralSettings.Static.MairDigitalSuiteDatabaseConnection);

            _context = new MairDigitalSuiteDatabaseContext(optionsBuilder.Options);
            _plcHub = plcHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var plcManager = new PlcManager(_context);

                    DashboardDataResult result;

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        result = await plcManager.GetDashboardData();

                        if (!result.Successful) throw new Exception($"Error {nameof(plcManager.GetDashboardData)} - {result.OriginalException.ToString()}");

                        await _plcHub.Clients.All.ShowDashboardData(result.Data);
                        await Task.Delay(1000);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    throw ex;
                }
            }
        }
    }
}
