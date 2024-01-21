using Supp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supp.Common
{
    public class Utility
    {
        /// <summary>
        /// GetPartOfTheDay
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static PartsOfTheDayEng GetPartOfTheDay(DateTime dateTime)
        {
            var time = int.Parse(DateTime.Now.ToString("HHmm"));
            var result = PartsOfTheDayEng.NotSet;

            if (time >= 600 && time <= 1159) result = PartsOfTheDayEng.Morning;
            if (time >= 1200 && time <= 1759) result = PartsOfTheDayEng.Afternoon;
            if (time >= 1800 && time <= 2359) result = PartsOfTheDayEng.Evening;
            if (time >= 0 && time <= 559) result = PartsOfTheDayEng.Night;

            return result;
        }

        /// <summary>
        /// GetSalutation
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string GetSalutation(CultureInfo cultureInfo)
        {
            Random rnd = new Random();
            var result = "";
            var now = DateTime.Now;

            if (Supp.Common.Utility.GetPartOfTheDay(now) == PartsOfTheDayEng.Morning)
            {
                int x = rnd.Next(0, 10 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buongiorno.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good morning.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona giornata.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice day.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = "Buona giornata.";
                if (cultureInfo.Name == "en-US" && x == 2) result = "Good day.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = ". Ti auguro una splendida giornata.";
                if (cultureInfo.Name == "en-US" && x == 3) result = ". Have a beautiful day.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = "Splendida giornata.";
                if (cultureInfo.Name == "en-US" && x == 4) result = "Beautiful day.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = ". Ti auguro una meravigliosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 5) result = ". Have a marvelous day.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = "Meravigliosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 6) result = "Marvelous day.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = ". Ti auguro una stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = ". Have a stupendous day.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = "Stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 8) result = "Stupendous day.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = ". Ti auguro una strepitosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 9) result = ". Have a amazing day.";

                if (cultureInfo.Name == "it-IT" && x == 10) result = "Strepitosa giornata.";
                if (cultureInfo.Name == "en-US" && x == 10) result = "Amazing day.";
            }

            if (Supp.Common.Utility.GetPartOfTheDay(now) == PartsOfTheDayEng.Afternoon)
            {
                int x = rnd.Next(0, 9 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buon pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro un buon pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = ". Ti auguro un splendido pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 2) result = ". Have a beautiful afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = "Splendido pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 3) result = "Beautiful afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = ". Ti auguro un meraviglioso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 4) result = ". Have a marvelous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = "Meraviglioso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 5) result = "Marvelous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = ". Ti auguro un stupendo pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 6) result = ". Have a stupendous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = "Stupenda giornata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = "Stupendous afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = ". Ti auguro un strepitoso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 8) result = ". Have a amazing afternoon.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = "Strepitoso pomeriggio.";
                if (cultureInfo.Name == "en-US" && x == 9) result = "Amazing afternoon.";
            }

            if (Supp.Common.Utility.GetPartOfTheDay(now) == PartsOfTheDayEng.Evening)
            {
                int x = rnd.Next(0, 10 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buonasera.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good evening.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona serata.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice evening.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = "Buona serata.";
                if (cultureInfo.Name == "en-US" && x == 2) result = "Good evening.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = ". Ti auguro una splendida serata.";
                if (cultureInfo.Name == "en-US" && x == 3) result = ". Have a beautiful evening.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = "Splendida serata.";
                if (cultureInfo.Name == "en-US" && x == 4) result = "Beautiful evening.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = ". Ti auguro una meravigliosa serata.";
                if (cultureInfo.Name == "en-US" && x == 5) result = ". Have a marvelous evening.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = "Meravigliosa serata.";
                if (cultureInfo.Name == "en-US" && x == 6) result = "Marvelous evening.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = ". Ti auguro una stupenda serata.";
                if (cultureInfo.Name == "en-US" && x == 7) result = ". Have a stupendous evening.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = "Stupenda serata.";
                if (cultureInfo.Name == "en-US" && x == 8) result = "Stupendous evening.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = ". Ti auguro una strepitosa serata.";
                if (cultureInfo.Name == "en-US" && x == 9) result = ". Have a amazing evening.";

                if (cultureInfo.Name == "it-IT" && x == 10) result = "Strepitosa serata.";
                if (cultureInfo.Name == "en-US" && x == 10) result = "Amazing evening.";
            }

            if (Supp.Common.Utility.GetPartOfTheDay(now) == PartsOfTheDayEng.Night)
            {
                int x = rnd.Next(0, 9 + 1);
                if (cultureInfo.Name == "it-IT" && x == 0) result = "Buona notte.";
                if (cultureInfo.Name == "en-US" && x == 0) result = "Good night.";

                if (cultureInfo.Name == "it-IT" && x == 1) result = ". Ti auguro una buona notte.";
                if (cultureInfo.Name == "en-US" && x == 1) result = ". Have a nice night.";

                if (cultureInfo.Name == "it-IT" && x == 2) result = ". Ti auguro una splendida notte.";
                if (cultureInfo.Name == "en-US" && x == 2) result = ". Have a beautiful night.";

                if (cultureInfo.Name == "it-IT" && x == 3) result = "Splendida notte.";
                if (cultureInfo.Name == "en-US" && x == 3) result = "Beautiful night.";

                if (cultureInfo.Name == "it-IT" && x == 4) result = ". Ti auguro una meravigliosa notte.";
                if (cultureInfo.Name == "en-US" && x == 4) result = ". Have a marvelous night.";

                if (cultureInfo.Name == "it-IT" && x == 5) result = "Meravigliosa notte.";
                if (cultureInfo.Name == "en-US" && x == 5) result = "Marvelous night.";

                if (cultureInfo.Name == "it-IT" && x == 6) result = ". Ti auguro una stupenda notte.";
                if (cultureInfo.Name == "en-US" && x == 6) result = ". Have a stupendous night.";

                if (cultureInfo.Name == "it-IT" && x == 7) result = "Stupenda notte.";
                if (cultureInfo.Name == "en-US" && x == 7) result = "Stupendous night.";

                if (cultureInfo.Name == "it-IT" && x == 8) result = ". Ti auguro una strepitosa notte.";
                if (cultureInfo.Name == "en-US" && x == 8) result = ". Have a amazing night.";

                if (cultureInfo.Name == "it-IT" && x == 9) result = "Strepitosa notte.";
                if (cultureInfo.Name == "en-US" && x == 9) result = "Amazing night.";
            }

            return result;
        }
    }
}
