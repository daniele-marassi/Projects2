using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Supp.Models;
using System;
using System.Collections.Generic;

namespace Supp.Site.Common
{
    public class Config
    {
        public class Roles
        {
            public class Constants
            {
                public const string RoleAdmin = "Admin";

                public const string RoleUser = "User";

                public const string RoleSuperUser = "SuperUser";

                public const string RoleGuest = "Guest";
            }
        }

        public class GeneralSettings
        {
            public class Static
            {
                public static string BaseUrl { get; set; }
                public static int PageSize { get; set; }
                public static int LimitLogFileInMB { get; set; }
                public static string HostsArray { get; set; }
                public static string HostDefault { get; set; }
                public static string ListeningWord1 { get; set; }
                public static string ListeningWord2 { get; set; }
                public static string ListeningAnswer { get; set; }
                public static string Salutation { get; set; }
                public static string MeteoParameterToTheSalutation { get; set; }
                public static bool DescriptionMeteoToTheSalutationActive { get; set; }
                public static bool RemindersActive { get; set; }
                public static string Culture { get; set; }
                public static int TimeToResetInSeconds { get; set; }
                public static int TimeToEhiTimeoutInSeconds { get; set; }
                public static bool WakeUpScreenAfterEhiActive { get; set; }
                public static string GoogleCalendarAccount { get; set; }
                

                public static string ConfigDefaultInJson
                {
                    get
                    {
                        var config = new Configuration()
                        {
                            General = new Configuration._General()
                            {
                                PageSize = GeneralSettings.Static.PageSize.ToString(),
                                Culture = GeneralSettings.Static.Culture
                            },
                            Speech = new Configuration._Speech()
                            {
                                HostsArray = GeneralSettings.Static.HostsArray,
                                HostDefault = GeneralSettings.Static.HostDefault,
                                ListeningWord1 = GeneralSettings.Static.ListeningWord1,
                                ListeningWord2 = GeneralSettings.Static.ListeningWord2,
                                ListeningAnswer = GeneralSettings.Static.ListeningAnswer,
                                Salutation = GeneralSettings.Static.Salutation,
                                MeteoParameterToTheSalutation = GeneralSettings.Static.MeteoParameterToTheSalutation,
                                DescriptionMeteoToTheSalutationActive = GeneralSettings.Static.DescriptionMeteoToTheSalutationActive,
                                RemindersActive = GeneralSettings.Static.RemindersActive,
                                TimeToResetInSeconds = GeneralSettings.Static.TimeToResetInSeconds,
                                TimeToEhiTimeoutInSeconds = GeneralSettings.Static.TimeToEhiTimeoutInSeconds,
                                WakeUpScreenAfterEhiActive = GeneralSettings.Static.WakeUpScreenAfterEhiActive,
                                GoogleCalendarAccount = GeneralSettings.Static.GoogleCalendarAccount
                            }
                        };

                        var result = JsonConvert.SerializeObject(config);

                        var salutationNote = "/*Salutation - If it contains the key 'NAME' it will be replaced with your profile name. If it contains the key 'SURNAME' it will be replaced with your profile surname.*/";

                        //add salutation note
                        result = result.Replace("\"" + nameof(Salutation) + "\"", " " + salutationNote + " " + "\"" + nameof(Salutation) + "\"");

                        var meteoParameterToTheSalutationNote = "/*MeteoParameterToTheSalutation - empty to disable it.*/";

                        //add meteoParameterToTheSalutation note
                        result = result.Replace("\"" + nameof(MeteoParameterToTheSalutation) + "\"", " " + meteoParameterToTheSalutationNote + " " + "\"" + nameof(MeteoParameterToTheSalutation) + "\"");

                        return result;
                    }
                }
            }

            public class Constants
            {
                public const string DefaultPassword = "Password!123";
                public const string SuppSiteAccessTokenCookieName = "SuppSiteAccessToken";
                public const string SuppSiteAuthenticatedUserIdCookieName = "SuppSiteAuthenticatedUserId";
                public const string SuppSiteAuthenticatedUserNameCookieName = "SuppSiteAuthenticatedUserName";
                public const string SuppSiteErrorsCookieName = "SuppSiteErrors";
                public const string SuppSiteExpiresInSecondsCookieName = "SuppSiteExpiresInSeconds";
                public const string SuppSiteHostSelectedCookieName = "SuppSiteHostSelected";
                public const string SuppSiteApplicationCookieName = "SuppSiteApplication";
                public const string SuppSiteAlwaysShowCookieName = "SuppSiteAlwaysShow";
                public const string SuppSiteClaimsCookieName = "SuppSiteClaims";
                public const string SuppSiteLoadDateCookieName = "SuppSiteLoadDate";
                public const string SuppSiteAuthenticatedPasswordCookieName = "SuppSiteAuthenticatedPassword";
                public const string SuppSiteNewWebSpeechCookieName = "SuppSiteNewWebSpeech";
                public const string SuppSiteSongsPositionCookieName = "SuppSiteSongsPosition";
                public const string SuppSiteSongIdCookieName = "SuppSiteSongId";
                public const string SuppSitePlayListSelectedCookieName = "SuppSitePlayListSelected";
                public const string SuppSiteNewWebSpeechDtoInJsonCookieName = "SuppSiteNewWebSpeechDtoInJson";
                public const string SuppSiteTimerParamInJsonCookieName = "SuppSiteTimerParamInJson";
                public const string SuppSiteTimerLastIndexCookieName = "SuppSiteTimerLastIndex";
            }

            public static void SetGeneralSettings(IConfiguration configuration)
            {
                if (configuration != null)
                {
                    //system configs - START
                    GeneralSettings.Static.BaseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;
                    GeneralSettings.Static.LimitLogFileInMB = Int32.Parse(configuration.GetSection("AppSettings:LimitLogFileInMB").Value);
                    //system configs - END

                    GeneralSettings.Static.PageSize = Int32.Parse(configuration.GetSection("AppSettings:PageSize").Value);
                    GeneralSettings.Static.HostsArray = configuration.GetSection("AppSettings:HostsArray").Value;
                    GeneralSettings.Static.HostDefault = configuration.GetSection("AppSettings:HostDefault").Value;
                    GeneralSettings.Static.ListeningWord1 = configuration.GetSection("AppSettings:ListeningWord1").Value;
                    GeneralSettings.Static.ListeningWord2 = configuration.GetSection("AppSettings:ListeningWord2").Value;
                    GeneralSettings.Static.ListeningAnswer = configuration.GetSection("AppSettings:ListeningAnswer").Value;
                    
                    GeneralSettings.Static.Culture = configuration.GetSection("AppSettings:Culture").Value;
                    GeneralSettings.Static.MeteoParameterToTheSalutation = configuration.GetSection("AppSettings:MeteoParameterToTheSalutation").Value;
                    GeneralSettings.Static.DescriptionMeteoToTheSalutationActive = bool.Parse(configuration.GetSection("AppSettings:DescriptionMeteoToTheSalutationActive").Value);
                    GeneralSettings.Static.RemindersActive = bool.Parse(configuration.GetSection("AppSettings:RemindersActive").Value);
                    GeneralSettings.Static.TimeToResetInSeconds = int.Parse(configuration.GetSection("AppSettings:TimeToResetInSeconds").Value);
                    GeneralSettings.Static.TimeToEhiTimeoutInSeconds = int.Parse(configuration.GetSection("AppSettings:TimeToEhiTimeoutInSeconds").Value);
                    GeneralSettings.Static.RemindersActive = bool.Parse(configuration.GetSection("AppSettings:WakeUpScreenAfterEhiActive").Value);
                    GeneralSettings.Static.GoogleCalendarAccount = configuration.GetSection("AppSettings:GoogleCalendarAccount").Value;
                }
            }
        }
    }
}