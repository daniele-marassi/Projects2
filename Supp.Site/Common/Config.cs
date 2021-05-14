using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Supp.Site.Models;
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
                public static string HostSelected { get; set; }
                public static string ListeningWord1 { get; set; }
                public static string ListeningWord2 { get; set; }
                public static string ListeningAnswer { get; set; }
                public static string Salutation { get; set; }
                public static string Name { get; set; }
                public static string Surname { get; set; }
                public static string UserName { get; set; }
                public static decimal SpeechWordsCoefficient { get; set; }
                public static string Culture { get; set; }
                
                public static string ConfigDefaultInJson 
                { 
                    get 
                    {
                        var config = new Models.Configuration() 
                        { 
                            General = new Models.Configuration._General() 
                            {
                                PageSize = "3",
                                Culture = "it-IT"
                            },
                             Speech = new Models.Configuration._Speech()
                             {
                                    HostsArray = "[\"EV-PC\",\"EV-TB\"]",
                                    HostSelected = "EV-PC",
                                    ListeningWord1 = "ehi",
                                    ListeningWord2 = "box",
                                    ListeningAnswer = "si dimmi",
                                    Salutation = "Ehi NAME",
                                    SpeechWordsCoefficient = "0,6666666666666667"
                             }
                        };

                        var result = JsonConvert.SerializeObject(config);

                        var salutationNote = "/*Salutation - If it contains the key 'NAME' it will be replaced with your profile name. If it contains the key 'SURNAME' it will be replaced with your profile surname.*/";
                        
                        //add salutation note
                        result = result.Replace("\"" + nameof(Salutation) + "\"", " " + salutationNote + " " + "\"" + nameof(Salutation) + "\"");

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
                
            }

            public static void SetGeneralSettings(IConfiguration configuration, Configuration obj)
            {
                if (configuration != null)
                {
                    //system configs - START
                    GeneralSettings.Static.BaseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;
                    GeneralSettings.Static.LimitLogFileInMB = Int32.Parse(configuration.GetSection("AppSettings:LimitLogFileInMB").Value);
                    //system configs - END

                    GeneralSettings.Static.PageSize = Int32.Parse(configuration.GetSection("AppSettings:PageSize").Value);
                    GeneralSettings.Static.HostsArray = configuration.GetSection("AppSettings:HostsArray").Value;
                    GeneralSettings.Static.HostSelected = configuration.GetSection("AppSettings:HostSelected").Value;
                    GeneralSettings.Static.ListeningWord1 = configuration.GetSection("AppSettings:ListeningWord1").Value;
                    GeneralSettings.Static.ListeningWord2 = configuration.GetSection("AppSettings:ListeningWord2").Value;
                    GeneralSettings.Static.ListeningAnswer = configuration.GetSection("AppSettings:ListeningAnswer").Value;
                    GeneralSettings.Static.Salutation = configuration.GetSection("AppSettings:Salutation").Value;
                    GeneralSettings.Static.SpeechWordsCoefficient = decimal.Parse(configuration.GetSection("AppSettings:SpeechWordsCoefficient").Value);
                    GeneralSettings.Static.Culture = configuration.GetSection("AppSettings:Culture").Value;
                }

                if (obj != null)
                {
                    try
                    {
                        GeneralSettings.Static.PageSize = Int32.Parse(obj.General.PageSize);
                        GeneralSettings.Static.Culture = obj.General.Culture;

                        GeneralSettings.Static.HostsArray = obj.Speech.HostsArray;
                        GeneralSettings.Static.HostSelected = obj.Speech.HostSelected;
                        GeneralSettings.Static.ListeningWord1 = obj.Speech.ListeningWord1;
                        GeneralSettings.Static.ListeningWord2 = obj.Speech.ListeningWord2;
                        GeneralSettings.Static.ListeningAnswer = obj.Speech.ListeningAnswer;
                        GeneralSettings.Static.Salutation = obj.Speech.Salutation;
                        GeneralSettings.Static.SpeechWordsCoefficient = decimal.Parse(obj.Speech.SpeechWordsCoefficient);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}