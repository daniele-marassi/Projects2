using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendar;
using GoogleManagerModels;
using GoogleService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        GoogleCalendarUtility googleCalendarUtility;
        public Form1()
        {
            InitializeComponent();
            googleCalendarUtility = new GoogleCalendarUtility();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {                                                               
            var authInJson = @"{""installed"":{""client_id"":""982569746577-4lnrb3udcu2dqqk2u2mts0j2rqmiripd.apps.googleusercontent.com"",""project_id"":""ace-case-311111"",""auth_uri"":""https://accounts.google.com/o/oauth2/auth"",""token_uri"":""https://oauth2.googleapis.com/token"",""auth_provider_x509_cert_url"":""https://www.googleapis.com/oauth2/v1/certs"",""client_secret"":""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"",""redirect_uris"":[""urn:ietf:wg:oauth:2.0:oob"",""http://localhost""]}}";
            var auth = JsonConvert.DeserializeObject<Auth>(authInJson);

            var tokenFileInJson = @"{""FileName"":""Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi"",""Content"":""{\""access_token\"":\""ya29.A0ARrdaM_2ZE1ShYj7YhC4owlKR7h_qUg8VMsSzY5oD0bP-02Yqd5lgpO-GyiP4SLkAPeAFktahrPHoGB42-1vUem4szqJCCdwitpCRHnuQCfQTYjkZT8bLrN7YTaGmfx9mCIXs2XnoEqpcqNixCL5OqTfpccG\"",\""token_type\"":\""Bearer\"",\""expires_in\"":3599,\""refresh_token\"":\""1//09OJGSFLIxH7tCgYIARAAGAkSNwF-L9Irzxdd_yFmJLX9yU16YGH5UWYKttocnreDZlqvn__uV5aDF0Wz-_cc2xxi7Kawm8-H45Q\"",\""scope\"":\""https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.readonly\"",\""Issued\"":\""2022-01-26T23:45:02.592+01:00\"",\""IssuedUtc\"":\""2022-01-26T22:45:02.592Z\""}""}";
            var tokenFile = JsonConvert.DeserializeObject<TokenFile>(tokenFileInJson);

            var googlePublicKey = "AIzaSyCdWVUdy3QmmYLjDwQWqP03gV49hfvWMhc";

            //var createCalendarEventRequest = new CreateCalendarEventRequest() 
            //{ 
            //    Auth = auth,
            //    TokenFile = tokenFile,
            //    Account = "daniele.marassi",
            //    Description = "Description 1",
            //    Location = "via giulia, 1 Trieste",
            //    Summary = "Summary 1",
            //    EventDateStart = DateTime.Parse("2021-12-01 23:45:00"),
            //    EventDateEnd = DateTime.Parse("2021-12-01 23:50:00"),
            //    NotificationMinutes = new List<int?>() {5, 10 },
            //    Color = GoogleCalendarColors.Blueberry
            //};

            //var calendarEvent = googleCalendarUtility.CreateCalendarEvent(createCalendarEventRequest);

            //var getCalendarEventsRequest = new CalendarEventsRequest()
            //{
            //    Auth = auth,
            //    TokenFile = tokenFile,
            //    Account = "daniele.marassi",
            //    TimeMin = DateTime.Parse("2021-12-10 00:00:00"),
            //    TimeMax = DateTime.Parse("2021-12-10 23:59:59")
            //};

            //var calendarEventsResult = googleCalendarUtility.GetCalendarEvents(getCalendarEventsRequest);

            //var ttt = 0;

            //var editCalendarEventRequest = new EditCalendarEventRequest()
            //{
            //    Auth = auth,
            //    TokenFile = tokenFile,
            //    Account = "daniele.marassi",

            //    //Description = "Description 1",
            //    //Location = "via giulia, 1 Trieste",
            //    //Summary = "Summary 1",
            //    //EventDateStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"),
            //    //EventDateEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"),
            //    //NotificationMinutes = new List<int?>() { 5, 10 },
            //    //Color = GoogleCalendarColors.Blueberry,
            //    TimeMin = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00:00"),
            //    TimeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"),

            //    SummaryToSearch = "#Note lista spesa",
            //    Description = String.Empty,
            //    DescriptionAppended = false
            //};

            //var calendarEventUpdated = googleCalendarUtility.EditLastCalendarEventBySummary(editCalendarEventRequest);
            //var tt = 0;

            //var deleteCalendarEventsRequest = new DeleteCalendarEventsRequest()
            //{
            //    Auth = auth,
            //    TokenFile = tokenFile,
            //    Account = "daniele.marassi",
            //    TimeMin = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00:00"),
            //    TimeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"),
            //    Summary = "Summary 1"
            //};

            //var eventDeleted = googleCalendarUtility.DeleteLastCalendarEventBySummary(deleteCalendarEventsRequest);

            //var holidays = await googleCalendarUtility.GetHolidays(CountryAndLanguageType.it_italian, googlePublicKey, DateTime.Parse("2021-12-01"), DateTime.Parse("2021-12-31"));

            //if (calendarEventsResult != null)
            //    if (holidays != null)
            //    {
            //        foreach (var holiday in holidays.Data)
            //        {
            //            calendarEventsResult.Data.Add( new CalendarEvent() { Summary = holiday.Summary, Description = holiday.Description, EventDateStart = holiday.Start.Date, IsHoliday= holiday.IsHoliday});
            //        } 
            //    }

            //foreach (var item in calendarEventsResult.Data.OrderBy(_=>_.EventDateStart))
            //{
            //    listBox1.Items.Add(item.Summary);
            //}

            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-7);
            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            var calendarEventsRequest = new CalendarEventsRequest()
            {
                Auth = auth,
                TokenFile = tokenFile,
                Account = "daniele.marassi",
                TimeMin = timeMin,
                TimeMax = timeMax
            };

            var getCalendarEventsResult = googleCalendarUtility.GetCalendarEvents(calendarEventsRequest);

            if (getCalendarEventsResult.Successful)
            {
                foreach (var item in getCalendarEventsResult.Data.Where(_ => _.Summary.Contains("#Note")).ToList())
                {
                    //var editCalendarEventRequest = new EditCalendarEventRequest()
                    //{
                    //    Auth = auth,
                    //    TokenFile = tokenFile,
                    //    Account = "daniele.marassi",
                    //    IdToSearch = item.Id,
                    //    EventDateStart = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00"),
                    //    EventDateEnd = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 23:59:59"),
                    //    TimeMin = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00"),
                    //    TimeMax = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 23:59:59")
                    //};

                    //var calendarEventUpdated = googleCalendarUtility.EditCalendarEventById(editCalendarEventRequest);

                    var ttt = 0;
                }
            }
        }
    }
}
