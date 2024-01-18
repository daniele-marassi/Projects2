using Supp.Models;
using System;
using System.Collections.Generic;
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
    }
}
