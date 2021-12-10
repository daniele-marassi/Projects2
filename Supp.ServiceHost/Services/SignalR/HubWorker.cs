using Supp.ServiceHost.Common;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Supp.ServiceHost.Common.Config;
using SuppModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Supp.ServiceHost.Services.Token;
using System.Collections.Generic;
using System.Linq;
using Additional.NLog;

namespace Supp.ServiceHost.Services.SignalR
{
    public class HubWorker: BackgroundService
    {
        private readonly IHubContext<Hub, IHub> _plcHub;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private SuppDatabaseContext _context;

        public HubWorker(IHubContext<Hub, IHub> plcHub)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SuppDatabaseContext>();

            optionsBuilder.UseSqlServer(GeneralSettings.Static.SuppDatabaseConnection, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            _context = new SuppDatabaseContext(optionsBuilder.Options);
            _plcHub = plcHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    //... = new ...(_context);

                    //...Result result;

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        //result = await ...();

                        //if (!result.Successful) throw new Exception($"Error {nameof(...)} - {result.OriginalException.ToString()}");

                        //await _plcHub.Clients.All.ShowHubData(result.Data);
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
