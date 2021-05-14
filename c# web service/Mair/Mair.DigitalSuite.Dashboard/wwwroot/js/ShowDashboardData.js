'use strict';

var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl(serverBaseUrl + '/hubs/plcData' + '?access_token=' + GetCookie('MairDigitalSuiteAccessToken'), {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .build();

connection.on('ShowDashboardData', function (plcDataDto) {
    try {
        CreateChartAndDetail(plcDataDto);
    } catch (err) {
        console.error(err.toString());
    }
});

connection.start().then(function () {
    return console.info('SignalR started!');
}).catch(function (err) {
    return console.error(err.toString());
});
