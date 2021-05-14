using Mair.DigitalSuite.Dashboard.Contracts;
using Mair.DigitalSuite.Dashboard.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mair.DigitalSuite.Dashboard.Common;
using Microsoft.JSInterop;
using Mair.DigitalSuite.Dashboard.Models.Dto;

namespace Mair.DigitalSuite.Dashboard.Services.SignalR
{
    public partial class PlcHubClient : IPlcHubClient, IHostedService
    {
        private readonly HubConnection _connection;
        private readonly IJSRuntime _jsRuntime;
        private readonly string _jsMethodName;

        public PlcHubClient(IJSRuntime jsRuntime, string token, string jsMethodName)
        {
            _jsMethodName = jsMethodName;
            _jsRuntime = jsRuntime;
            _connection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(Config.GeneralSettings.Static.BaseUrl) + "hubs/plcData"),
                HttpTransportType.WebSockets, options =>
                {
                    options.Headers.Add("access_token", token);
                })
                .Build();

            _connection.On<List<DashBoardDataDto>>("ShowDashboardData",
                dto => _ = Execute(dto));
        }

        public Task Execute(List<DashBoardDataDto> dashBoardDataDto)
        {
            _jsRuntime.InvokeVoidAsync(_jsMethodName, dashBoardDataDto);
            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Loop is here to wait until the server is running
            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);
                    break;
                }
                catch
                {
                    await Task.Delay(1000);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _connection.DisposeAsync();
        }
    }
}
