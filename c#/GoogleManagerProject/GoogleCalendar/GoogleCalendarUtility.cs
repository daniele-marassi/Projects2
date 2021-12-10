using Additional;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleManagerModels;
using GoogleService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar
{
    public class GoogleCalendarUtility
    {
        /// <summary>
        /// Create Calendar Event
        /// </summary>
        /// <param name="createCalendarEventRequest"></param>
        /// <returns></returns>
        public EventResult CreateCalendarEvent(CreateCalendarEventRequest createCalendarEventRequest)
        {
            var result = new EventResult() { Data = new List<Event>() { }, Successful = false, ResultState = ResultType.None };

            try
            {
                var _service = GetService(createCalendarEventRequest.TokenFile, createCalendarEventRequest.Auth, createCalendarEventRequest.Account);

                string colorId = null;
                var start = new EventDateTime();
                var end = new EventDateTime();

                colorId = ((int)createCalendarEventRequest.Color).ToString();

                if (colorId == ((int)GoogleCalendarColors.Default).ToString())
                    colorId = null;

                start.DateTime = createCalendarEventRequest.EventDateStart;

                end.DateTime = createCalendarEventRequest.EventDateEnd;

                var ev = new Event();
                ev.Start = start;
                ev.End = end;
                ev.Summary = createCalendarEventRequest.Summary;
                ev.Description = createCalendarEventRequest.Description;
                ev.Location = createCalendarEventRequest.Location;
                ev.ColorId = colorId;

                var reminders = new Event.RemindersData() { Overrides = new List<EventReminder>() { }, UseDefault = false };

                foreach (var item in createCalendarEventRequest.NotificationMinutes)
                {
                    reminders.Overrides.Add(new EventReminder() { Minutes = item, ETag = null, Method = "popup" });
                }

                ev.Reminders = reminders;

                var calendarId = "primary";
                var recurringEvent = _service.Events.Insert(ev, calendarId).ExecuteAsync().GetAwaiter().GetResult();

                if (recurringEvent != null)
                {
                    result.Data.Add(recurringEvent);
                    result.Successful = true;
                    result.ResultState = ResultType.Found;
                    result.Message = null;
                }
                else
                {
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get Service
        /// </summary>
        /// <param name="tokenFile"></param>
        /// <param name="auth"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public CalendarService GetService(TokenFile tokenFile, Auth auth, string account)
        {
            var result = new CalendarService();

            try
            {
                var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var accessProperties = JsonConvert.DeserializeObject<AccessProperties>(tokenFile.Content);
                accessProperties.Expires_in = 3599;

                var resourcesPath = Path.Combine(appPath, "Resources");
                if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
                var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                var tokenFileFullPath = Path.Combine(tokenFilePath, tokenFile.FileName);

                if (System.IO.File.Exists(tokenFileFullPath)) System.IO.File.Delete(tokenFileFullPath);

                System.IO.File.WriteAllText(tokenFileFullPath, JsonConvert.SerializeObject(accessProperties));

                var googleServiceUtility = new GoogleServiceUtility();

                var managerRequest = new ManagerRequest() { Auth = auth, Account = account };

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleServiceUtility.CreateCredential(managerRequest, AccountType.Calendar),
                    ApplicationName = "Google Calendar",
                });

                result = service;
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Get Calendar Events
        /// </summary>
        /// <param name="getCalendarEventsRequest"></param>
        /// <returns></returns>
        public CalendarEventsResult GetCalendarEvents(CalendarEventsRequest getCalendarEventsRequest)
        {
            var result = new CalendarEventsResult() { Data = new List<CalendarEvent>() { } , Successful = false, ResultState = ResultType.None  };

            try
            {
                var _service = GetService(getCalendarEventsRequest.TokenFile, getCalendarEventsRequest.Auth, getCalendarEventsRequest.Account);

                // Define parameters of request.
                EventsResource.ListRequest _request = _service.Events.List("primary");
                _request.TimeMin = getCalendarEventsRequest.TimeMin;
                _request.TimeMax = getCalendarEventsRequest.TimeMax;
                _request.ShowDeleted = false;
                _request.SingleEvents = true;
                _request.ShowHiddenInvitations = true;
                //_request.MaxResults = 3000;
                _request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        var when = eventItem.Start.DateTime.ToString();
                        DateTime? eventDate = null;
                        var notificationDates = new List<DateTime>() { };

                        if (String.IsNullOrEmpty(when))
                            when = eventItem.Start.Date;

                        if (!String.IsNullOrEmpty(when))
                            eventDate = DateTime.Parse(when);

                        if (eventItem.Reminders.Overrides != null)
                        {
                            foreach (var item in eventItem.Reminders.Overrides)
                            {
                                if (eventDate != null)
                                    notificationDates.Add(eventDate.Value.AddMinutes(-(int)item.Minutes));
                            }
                        }
                        result.Data.Add( new CalendarEvent()
                        {
                            Id = eventItem.Id,
                            Description = eventItem.Description != null ? eventItem.Description : String.Empty,
                            Location = eventItem.Location != null ? eventItem.Location : String.Empty,
                            Summary = eventItem.Summary != null ? eventItem.Summary : String.Empty,
                            EventDateStart = eventDate,
                            NotificationDates = notificationDates
                        });
                    }

                    result.Successful = true;
                    result.ResultState = ResultType.Found;
                    result.Message = null;
                }
                else
                {
                    //"No upcoming events found."
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Delete Last Calendar Event By Summary
        /// </summary>
        /// <param name="deleteCalendarEventsRequest"></param>
        /// <returns></returns>
        public EventResult DeleteLastCalendarEventBySummary(DeleteCalendarEventsRequest deleteCalendarEventsRequest)
        {
            var result = new EventResult() { Data = new List<Event>() { }, Successful = false, ResultState = ResultType.None };

            try
            {
                var _service = GetService(deleteCalendarEventsRequest.TokenFile, deleteCalendarEventsRequest.Auth, deleteCalendarEventsRequest.Account);

                // Define parameters of request.
                EventsResource.ListRequest _request = _service.Events.List("primary");
                _request.TimeMin = deleteCalendarEventsRequest.TimeMin;
                _request.TimeMax = deleteCalendarEventsRequest.TimeMax;
                _request.ShowDeleted = false;
                _request.SingleEvents = true;
                _request.ShowHiddenInvitations = true;
                //_request.MaxResults = 3000;
                _request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items.OrderBy(_ => _.Created).ToList())
                    {
                        if (eventItem.Summary.ToLower().Trim() == deleteCalendarEventsRequest.Summary.ToLower().Trim())
                            result.Data.Add(eventItem);
                    }

                    result.Successful = true;
                    result.ResultState = ResultType.Found;
                    result.Message = null;
                }
                else
                {
                    //"No events found."
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }

                if (result.Data.Count > 0)
                {
                    try
                    {
                        _service.Events.Delete(_request.CalendarId, result.Data.LastOrDefault().Id).ExecuteAsync().GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        result.Successful = false;
                        result.ResultState = ResultType.Error;
                        result.Message = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Edit Last Calendar Event By Summary
        /// </summary>
        /// <param name="editCalendarEventRequest"></param>
        /// <returns></returns>
        public EventResult EditLastCalendarEventBySummary(EditCalendarEventRequest editCalendarEventRequest)
        {
            Event currentEvent = null;
            Event updatedEvent = null;

            var result = new EventResult() { Data = new List<Event>() { }, Successful = false, ResultState = ResultType.None };

            try
            {
                var _service = GetService(editCalendarEventRequest.TokenFile, editCalendarEventRequest.Auth, editCalendarEventRequest.Account);

                // Define parameters of request.
                EventsResource.ListRequest _request = _service.Events.List("primary");
                if (editCalendarEventRequest.TimeMin != null) _request.TimeMin = editCalendarEventRequest.TimeMin;
                if (editCalendarEventRequest.TimeMax != null) _request.TimeMax = editCalendarEventRequest.TimeMax;
                _request.ShowDeleted = false;
                _request.SingleEvents = true;
                _request.ShowHiddenInvitations = true;
                //_request.MaxResults = 3000;
                _request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items.OrderBy(_ => _.Created).ToList())
                    {
                        if (eventItem.Summary.ToLower().Trim() == editCalendarEventRequest.SummaryToSearch.ToLower().Trim())
                            currentEvent = eventItem;
                    }
                }
                else
                {
                    //"No events found."
                    currentEvent = null;
                }

                if (currentEvent != null)
                {
                    try
                    {
                        string colorId = null;
                        var start = new EventDateTime();
                        var end = new EventDateTime();

                        if (editCalendarEventRequest.Color != null)
                        {
                            colorId = ((int)editCalendarEventRequest.Color).ToString();

                            if (colorId == ((int)GoogleCalendarColors.Default).ToString())
                                colorId = null;
                        }
                        else
                            colorId = currentEvent.ColorId;

                        if (editCalendarEventRequest.EventDateStart != null)
                            start.DateTime = editCalendarEventRequest.EventDateStart;
                        else
                            start.DateTime = currentEvent.Start.DateTime;

                        if (editCalendarEventRequest.EventDateEnd != null)
                            end.DateTime = editCalendarEventRequest.EventDateEnd;
                        else
                            end.DateTime = currentEvent.End.DateTime;

                        var ev = new Event();
                        ev.Start = start;
                        ev.End = end;

                        if (editCalendarEventRequest.Summary != null)
                            ev.Summary = editCalendarEventRequest.Summary;
                        else
                            ev.Summary = currentEvent.Summary;

                        if (editCalendarEventRequest.Description != null)
                            ev.Description = editCalendarEventRequest.Description;
                        else
                            ev.Description = currentEvent.Description;

                        if (editCalendarEventRequest.Location != null)
                            ev.Location = editCalendarEventRequest.Location;
                        else
                            ev.Location = currentEvent.Location;

                        ev.ColorId = colorId;

                        if (editCalendarEventRequest.NotificationMinutes != null)
                        {
                            var reminders = new Event.RemindersData() { Overrides = new List<EventReminder>() { }, UseDefault = false };

                            foreach (var item in editCalendarEventRequest.NotificationMinutes)
                            {
                                reminders.Overrides.Add(new EventReminder() { Minutes = item, ETag = null, Method = "popup" });
                            }

                            ev.Reminders = reminders;
                        }
                        else
                            ev.Reminders = currentEvent.Reminders;

                        updatedEvent = _service.Events.Update(ev, _request.CalendarId, currentEvent.Id).ExecuteAsync().GetAwaiter().GetResult();

                        if (updatedEvent != null)
                        {
                            result.Data.Add(updatedEvent);
                            result.Successful = true;
                            result.ResultState = ResultType.Found;
                            result.Message = null;
                        }
                        else
                        {
                            result.Successful = true;
                            result.ResultState = ResultType.NotFound;
                            result.Message = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Successful = false;
                        result.ResultState = ResultType.Error;
                        result.Message = ex.Message;
                    }
                }
                else
                {
                    //"No events found."
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get Holidays
        /// </summary>
        /// <param name="countryAndLanguageType"></param>
        /// <param name="key"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public async Task<HolidaysResult> GetHolidays(CountryAndLanguageType countryAndLanguageType, string key, DateTime dateStart, DateTime dateEnd)
        {
            var result = new HolidaysResult() { Data = new List<Holiday>() { }, Successful = false, ResultState = GoogleManagerModels.ResultType.None };

            var utility = new Utility();

            var countryAndLanguage = countryAndLanguageType.ToString().Replace("_", ".");

            var baseUrl = "https://www.googleapis.com/calendar/v3/calendars/" + countryAndLanguage + "%23holiday%40group.v.calendar.google.com/";

            var apiUrl = "events?key=" + key;

            var response = await utility.CallApi(HttpMethod.Get, baseUrl, apiUrl, null);
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                HolidaysApiResult holidaysApiResult = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    holidaysApiResult = JsonConvert.DeserializeObject<HolidaysApiResult>(content);
                }

                try
                {
                    if (holidaysApiResult != null && holidaysApiResult.Items.Count > 0)
                    {
                        var items = holidaysApiResult.Items.Where(_ =>
                        _.Start.Date >= DateTime.Parse(dateStart.Year.ToString() + "-" + dateStart.Month.ToString() + "-" + dateStart.Day.ToString() + " 00:00:00")
                        &&
                        _.Start.Date <= DateTime.Parse(dateEnd.Year.ToString() + "-" + dateEnd.Month.ToString() + "-" + dateEnd.Day.ToString() + " 23:59:59")
                        ).OrderBy(_ => _.Start.Date).ToList();

                        result.Data = items;
                        result.Successful = true;
                        result.ResultState = GoogleManagerModels.ResultType.Found;
                        result.Message = null;
                    }
                    else if (holidaysApiResult != null)
                    {
                        result.Successful = true;
                        result.ResultState = GoogleManagerModels.ResultType.NotFound;
                        result.Message = null;
                    }
                    else 
                    {
                        result.Successful = false;
                        result.ResultState = GoogleManagerModels.ResultType.Error;
                        result.Message = response.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    result.Successful = false;
                    result.ResultState = GoogleManagerModels.ResultType.Error;
                    result.Message = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = GoogleManagerModels.ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get Last Calendar Event By Summary
        /// </summary>
        /// <param name="getCalendarEventsRequest"></param>
        /// <returns></returns>
        public EventResult GetLastCalendarEventBySummary(GetCalendarEventsRequest getCalendarEventsRequest)
        {
            var result = new EventResult() { Data = new List<Event>() { }, Successful = false, ResultState = ResultType.None };

            try
            {
                var _service = GetService(getCalendarEventsRequest.TokenFile, getCalendarEventsRequest.Auth, getCalendarEventsRequest.Account);

                // Define parameters of request.
                EventsResource.ListRequest _request = _service.Events.List("primary");
                _request.TimeMin = getCalendarEventsRequest.TimeMin;
                _request.TimeMax = getCalendarEventsRequest.TimeMax;
                _request.ShowDeleted = false;
                _request.SingleEvents = true;
                _request.ShowHiddenInvitations = true;
                //_request.MaxResults = 3000;
                _request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items.OrderBy(_ => _.Created).ToList())
                    {
                        if (eventItem.Summary.ToLower().Trim() == getCalendarEventsRequest.Summary.ToLower().Trim())
                            result.Data.Add(eventItem);
                    }

                    result.Successful = true;
                    result.ResultState = ResultType.Found;
                    result.Message = null;
                }
                else
                {
                    //"No events found."
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public EventResult EditCalendarEventById(EditCalendarEventRequest editCalendarEventRequest)
        {
            Event currentEvent = null;
            Event updatedEvent = null;

            var result = new EventResult() { Data = new List<Event>() { }, Successful = false, ResultState = ResultType.None };

            try
            {
                var _service = GetService(editCalendarEventRequest.TokenFile, editCalendarEventRequest.Auth, editCalendarEventRequest.Account);

                // Define parameters of request.
                EventsResource.ListRequest _request = _service.Events.List("primary");
                if (editCalendarEventRequest.TimeMin != null) _request.TimeMin = editCalendarEventRequest.TimeMin;
                if (editCalendarEventRequest.TimeMax != null) _request.TimeMax = editCalendarEventRequest.TimeMax;
                _request.ShowDeleted = false;
                _request.SingleEvents = true;
                _request.ShowHiddenInvitations = true;
                //_request.MaxResults = 3000;
                _request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = _request.ExecuteAsync().GetAwaiter().GetResult();

                if (events.Items != null && events.Items.Count > 0)
                {
                    currentEvent = events.Items.Where(_ => _.Id == editCalendarEventRequest.IdToSearch).FirstOrDefault();
                }

                if (currentEvent != null)
                {
                    try
                    {
                        string colorId = null;
                        var start = new EventDateTime();
                        var end = new EventDateTime();

                        if (editCalendarEventRequest.Color != null)
                        {
                            colorId = ((int)editCalendarEventRequest.Color).ToString();

                            if (colorId == ((int)GoogleCalendarColors.Default).ToString())
                                colorId = null;
                        }
                        else
                            colorId = currentEvent.ColorId;

                        if (editCalendarEventRequest.EventDateStart != null)
                            start.DateTime = editCalendarEventRequest.EventDateStart;
                        else
                            start.DateTime = currentEvent.Start.DateTime;

                        if (editCalendarEventRequest.EventDateEnd != null)
                            end.DateTime = editCalendarEventRequest.EventDateEnd;
                        else
                            end.DateTime = currentEvent.End.DateTime;

                        var ev = new Event();
                        ev.Start = start;
                        ev.End = end;

                        if (editCalendarEventRequest.Summary != null)
                            ev.Summary = editCalendarEventRequest.Summary;
                        else
                            ev.Summary = currentEvent.Summary;

                        if (editCalendarEventRequest.Description != null)
                            ev.Description = editCalendarEventRequest.Description;
                        else
                            ev.Description = currentEvent.Description;

                        if (editCalendarEventRequest.Location != null)
                            ev.Location = editCalendarEventRequest.Location;
                        else
                            ev.Location = currentEvent.Location;

                        ev.ColorId = colorId;

                        if (editCalendarEventRequest.NotificationMinutes != null)
                        {
                            var reminders = new Event.RemindersData() { Overrides = new List<EventReminder>() { }, UseDefault = false };

                            foreach (var item in editCalendarEventRequest.NotificationMinutes)
                            {
                                reminders.Overrides.Add(new EventReminder() { Minutes = item, ETag = null, Method = "popup" });
                            }

                            ev.Reminders = reminders;
                        }
                        else
                            ev.Reminders = currentEvent.Reminders;

                        updatedEvent = _service.Events.Update(ev, _request.CalendarId, currentEvent.Id).ExecuteAsync().GetAwaiter().GetResult();

                        if (updatedEvent != null)
                        {
                            result.Data.Add(updatedEvent);
                            result.Successful = true;
                            result.ResultState = ResultType.Found;
                            result.Message = null;
                        }
                        else
                        {
                            result.Successful = true;
                            result.ResultState = ResultType.NotFound;
                            result.Message = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Successful = false;
                        result.ResultState = ResultType.Error;
                        result.Message = ex.Message;
                    }
                }
                else
                {
                    //"No events found."
                    result.Successful = true;
                    result.ResultState = ResultType.NotFound;
                    result.Message = null;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.ResultState = ResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}