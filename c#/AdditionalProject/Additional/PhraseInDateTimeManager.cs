using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Additional
{
    public class PhraseInDateTimeManager
    {
        private Utility utility;

        public PhraseInDateTimeManager()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Convert Phrase In DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public DateTime? Convert(string value, string culture)
        {
            DateTime? result = null;

            try
            {
                if (culture.ToLower() == "it-it")
                    result = DateTime.Parse(value, new CultureInfo("it-IT"));

                if (culture.ToLower() == "en-us")
                    result = DateTime.Parse(value, new CultureInfo("en-US"));

                return result;
            }
            catch (Exception)
            {
            }

            string[] words = null;
            var _word = String.Empty;
            var fixedDay = false;
            var fixedMonth = false;
            var fixedYear = false;
            var fixedHour = false;
            var fixedMinute = false;
            var fixedSecond = false;
            var fixedQuantity = false;
            var fixedQuantityPeriod = false;
            var fixedWeek = false;

            var isNumericDay = false;
            var isNumericMonth = false;
            var isNumericYear = false;
            var isNumericHour = false;
            var isNumericMinute = false;
            var isDayOfWeek = false;
            var isDay = false;
            var isQuantity = false;
            var isQuantityPeriod = false;
            var isMonth = false;
            var isYear = false;
            var isSecond = false;
            var isMinute = false;
            var isHour = false;
            var isWeek = false;

            var isHalf = false;
            var isQuarter = false;
            var isMonthInWords = false;
            var isTimeAsNow = false;

            var isPM = false;
            var isAM = false;

            double quantity = 0;
            double quantityPeriod = 0;

            var date = DateTime.Now;

            var dayOfWeek = (int)date.DayOfWeek;

            if (culture.ToLower() == "it-it")
            {
                value = value.Replace("'", " ");
                value = value.Replace(":", " e ");

                if (value.ToLower().Contains(" e " + QuantityInWordsIta.Mezzo.ToString().ToLower()))
                {
                    isHalf = true;
                    value = value.ToLower().Replace(" e " + QuantityInWordsIta.Mezzo.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" e " + QuantityInWordsIta.Mezza.ToString().ToLower()))
                {
                    isHalf = true;
                    value = value.ToLower().Replace(" e " + QuantityInWordsIta.Mezza.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" e " + QuantityInWordsIta.Quarto.ToString().ToLower()))
                {
                    isQuarter = true;
                    value = value.ToLower().Replace(" e " + QuantityInWordsIta.Quarto.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" e " + QuantityInWordsIta.Quarti.ToString().ToLower()))
                {
                    isQuarter = true;
                    value = value.ToLower().Replace(" e " + QuantityInWordsIta.Quarti.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsIta.QuestaOra.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsIta.QuestaOra.ToString()).ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsIta.QuestOra.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsIta.QuestOra.ToString()).ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsIta.StessOra.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsIta.StessOra.ToString()).ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsIta.StessaOra.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsIta.StessaOra.ToString()).ToLower(), "");
                }

                if (isTimeAsNow)
                    date = DateTime.Parse(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second, new CultureInfo("it-IT"));
                else
                    date = DateTime.Parse(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " 00:00:11", new CultureInfo("it-IT"));

                words = value.Split(' ');

                foreach (var word in words)
                {
                    _word = word;

                    if (_word.Trim().ToLower() == QuantityInWordsIta.Un.ToString().ToLower() || _word.Trim().ToLower() == QuantityInWordsIta.Una.ToString().ToLower()) _word = "1";

                    DateTimeInNumbers(ref isNumericDay, ref fixedDay, ref isNumericMonth, ref fixedMonth, ref isNumericYear, ref fixedYear, ref isNumericHour, ref fixedHour, ref isNumericMinute, ref fixedMinute, ref date, _word, culture, isHalf, isQuarter, isAM, isPM);
                    DayOfWeekInWords(ref isDayOfWeek, ref fixedDay, ref isQuantity, ref fixedQuantity, ref quantity, ref date, _word, dayOfWeek, culture, isHalf, isQuarter, isAM, isPM);
                    PeriodInWords(ref isDay, ref fixedDay, ref isQuantityPeriod, ref fixedQuantityPeriod, ref isMonth, ref fixedMonth, ref isYear, ref fixedYear, ref isSecond, ref fixedSecond, ref isMinute, ref fixedMinute, ref isHour, ref fixedHour, ref isWeek, ref fixedWeek, ref quantityPeriod, ref date, _word, culture, isHalf, isQuarter, isAM, isPM);
                    MonthInWords(ref isMonthInWords, ref fixedMonth, ref fixedDay, ref date, word, culture);
                }
            }

            if (culture.ToLower() == "en-us")
            {
                value = value.Replace("'", " ");
                value = value.Replace(":", " and ");

                value = value.ToLower().Replace(utility.SplitCamelCase(DaysEng.DayAfterTomorrow.ToString()).ToLower(), DaysEng.DayAfterTomorrow.ToString().ToLower());

                if (value.ToLower().Contains(" a.m.") || value.ToLower().Contains(" am"))
                {
                    isAM = true;
                    value = value.ToLower().Replace(" a.m.", "");
                    value = value.ToLower().Replace(" am", "");
                }

                if (value.ToLower().Contains(" p.m.") || value.ToLower().Contains(" pm"))
                {
                    isPM = true;
                    value = value.ToLower().Replace(" p.m.", "");
                    value = value.ToLower().Replace(" pm", "");
                }

                if (value.ToLower().Contains(" and a " + QuantityInWordsEng.Half.ToString().ToLower()))
                {
                    isHalf = true;
                    value = value.ToLower().Replace(" and a " + QuantityInWordsEng.Half.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" and a " + QuantityInWordsEng.Quarter.ToString().ToLower()))
                {
                    isQuarter = true;
                    value = value.ToLower().Replace(" and a " + QuantityInWordsEng.Quarter.ToString().ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsEng.ThisHour.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsEng.ThisHour.ToString()).ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsEng.SameTime.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsEng.SameTime.ToString()).ToLower(), "");
                }

                if (value.ToLower().Contains(" " + utility.SplitCamelCase(QuantityInWordsEng.SameHour.ToString()).ToLower()))
                {
                    isTimeAsNow = true;
                    value = value.ToLower().Replace(" " + utility.SplitCamelCase(QuantityInWordsEng.SameHour.ToString()).ToLower(), "");
                }

                if (isTimeAsNow)
                    date = DateTime.Parse(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second, new CultureInfo("it-IT"));
                else
                    date = DateTime.Parse(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " 00:00:11", new CultureInfo("it-IT"));

                words = value.Split(' ');

                foreach (var word in words)
                {
                    _word = word;

                    if (_word.Trim().ToLower() == QuantityInWordsEng.One.ToString().ToLower()) _word = "1";

                    var text = Regex.Replace(_word, "[0-9]", "");
                    if (text.Trim().ToLower() == "th") _word = _word.Trim().ToLower().Replace("th", "");

                    if (_word.Trim().ToLower() == QuantityInWordsEng.One.ToString().ToLower()) _word = "1";

                    DateTimeInNumbers(ref isNumericDay, ref fixedDay, ref isNumericMonth, ref fixedMonth, ref isNumericYear, ref fixedYear, ref isNumericHour, ref fixedHour, ref isNumericMinute, ref fixedMinute, ref date, _word, culture, isHalf, isQuarter, isAM, isPM);
                    DayOfWeekInWords(ref isDayOfWeek, ref fixedDay, ref isQuantity, ref fixedQuantity, ref quantity, ref date, _word, dayOfWeek, culture, isHalf, isQuarter, isAM, isPM);
                    PeriodInWords(ref isDay, ref fixedDay, ref isQuantityPeriod, ref fixedQuantityPeriod, ref isMonth, ref fixedMonth, ref isYear, ref fixedYear, ref isSecond, ref fixedSecond, ref isMinute, ref fixedMinute, ref isHour, ref fixedHour, ref isWeek, ref fixedWeek, ref quantityPeriod, ref date, _word, culture, isHalf, isQuarter, isAM, isPM);
                    MonthInWords(ref isMonthInWords, ref fixedMonth, ref fixedDay, ref date, word, culture);
                }
            }

            if (fixedDay || fixedMonth || fixedYear || fixedSecond || fixedMinute || fixedHour || fixedWeek) result = date;

            return result;
        }

        private void MonthInWords(ref bool isMonthInWords, ref bool fixedMonth, ref bool fixedDay, ref DateTime date, string word, string culture)
        {
            if (utility.GetEnumItems(typeof(MothsIta)).Contains(word) && fixedMonth == false) isMonthInWords = true;

            if (isMonthInWords && fixedMonth == false)
            {
                try
                {
                    int newMonth = 0;
                    var cultureInfo = default(CultureInfo);
                    if (culture.ToLower() == "it-it")
                        cultureInfo = new CultureInfo("it-IT");
                    if (culture.ToLower() == "en-us")
                        cultureInfo = new CultureInfo("en-US");

                    if (culture.ToLower() == "it-it")
                        newMonth = (int)(MothsIta)Enum.Parse(typeof(MothsIta), utility.FirstLetterToUpper(word.Trim()));
                    if (culture.ToLower() == "en-us")
                        newMonth = (int)(MothsEng)Enum.Parse(typeof(MothsEng), utility.FirstLetterToUpper(word.Trim()));

                    date = DateTime.Parse(date.Day + "/" + newMonth + "/" + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second, new CultureInfo("it-IT"));

                    fixedMonth = true;
                }
                catch (Exception)
                { }
            }
        }

        private void PeriodInWords(ref bool isDay, ref bool fixedDay, ref bool isQuantityPeriod, ref bool fixedQuantityPeriod, ref bool isMonth, ref bool fixedMonth, ref bool isYear, ref bool fixedYear, ref bool isSecond, ref bool fixedSecond, ref bool isMinute, ref bool fixedMinute, ref bool isHour, ref bool fixedHour, ref bool isWeek, ref bool fixedWeek, ref double quantityPeriod, ref DateTime date, string word, string culture, bool isHalf, bool isQuarter, bool isAM, bool isPM)
        {
            if (isQuantityPeriod && fixedQuantityPeriod == false)
            {
                try
                {
                    quantityPeriod = double.Parse(word.Trim().ToLower());

                    fixedQuantityPeriod = true;
                }
                catch (Exception)
                { }
            }

            if (culture.ToLower() == "it-it" && utility.GetEnumItems(typeof(TemporalComplementIta)).Contains(word.Trim().ToLower()) && fixedQuantityPeriod == false) isQuantityPeriod = true;
            if (culture.ToLower() == "en-us" && utility.GetEnumItems(typeof(TemporalComplementEng)).Contains(word.Trim().ToLower()) && fixedQuantityPeriod == false) isQuantityPeriod = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Giorni.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Giorno.ToString().ToLower() == word.Trim().ToLower()) && fixedDay == false && fixedQuantityPeriod == true) isDay = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Days.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Day.ToString().ToLower() == word.Trim().ToLower()) && fixedDay == false && fixedQuantityPeriod == true) isDay = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Mesi.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Mese.ToString().ToLower() == word.Trim().ToLower()) && fixedMonth == false && fixedQuantityPeriod == true) isMonth = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Months.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Month.ToString().ToLower() == word.Trim().ToLower()) && fixedMonth == false && fixedQuantityPeriod == true) isMonth = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Anni.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Anno.ToString().ToLower() == word.Trim().ToLower()) && fixedYear == false && fixedQuantityPeriod == true) isYear = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Years.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Year.ToString().ToLower() == word.Trim().ToLower()) && fixedYear == false && fixedQuantityPeriod == true) isYear = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Secondi.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Secondo.ToString().ToLower() == word.Trim().ToLower()) && fixedSecond == false && fixedQuantityPeriod == true) isSecond = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Seconds.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Second.ToString().ToLower() == word.Trim().ToLower()) && fixedSecond == false && fixedQuantityPeriod == true) isSecond = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Minuti.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Minuto.ToString().ToLower() == word.Trim().ToLower()) && fixedMinute == false && fixedQuantityPeriod == true) isMinute = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Minutes.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Minute.ToString().ToLower() == word.Trim().ToLower()) && fixedMinute == false && fixedQuantityPeriod == true) isMinute = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Ore.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Ora.ToString().ToLower() == word.Trim().ToLower()) && fixedHour == false && fixedQuantityPeriod == true) isHour = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Hours.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Hour.ToString().ToLower() == word.Trim().ToLower()) && fixedHour == false && fixedQuantityPeriod == true) isHour = true;

            if (culture.ToLower() == "it-it" && (TimeDefinitionsIta.Settimane.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsIta.Settimana.ToString().ToLower() == word.Trim().ToLower()) && fixedWeek == false && fixedQuantityPeriod == true) isWeek = true;
            if (culture.ToLower() == "en-us" && (TimeDefinitionsEng.Weeks.ToString().ToLower() == word.Trim().ToLower() || TimeDefinitionsEng.Week.ToString().ToLower() == word.Trim().ToLower()) && fixedWeek == false && fixedQuantityPeriod == true) isWeek = true;

            if (isDay && fixedQuantityPeriod && fixedDay == false)
            {
                date = DateTime.Now;
                if (isHalf) quantityPeriod += 0.5;
                if (isQuarter) quantityPeriod += 0.25;
                date = date.AddDays(quantityPeriod);

                fixedDay = true;
            }

            if (isMonth && fixedQuantityPeriod && fixedQuantityPeriod && fixedMonth == false)
            {
                date = DateTime.Now;
                date = date.AddMonths((int)quantityPeriod);

                fixedMonth = true;
            }

            if (isYear && fixedQuantityPeriod && fixedYear == false)
            {
                date = DateTime.Now;
                date = date.AddYears((int)quantityPeriod);

                fixedYear = true;
            }

            if (isSecond && fixedQuantityPeriod && fixedSecond == false)
            {
                date = DateTime.Now;
                if (isHalf) quantityPeriod += 0.5;
                if (isQuarter) quantityPeriod += 0.25;
                date = date.AddSeconds(quantityPeriod);

                fixedSecond = true;
            }

            if (isMinute && fixedQuantityPeriod && fixedMinute == false)
            {
                date = DateTime.Now;
                if (isHalf) quantityPeriod += 0.5;
                if (isQuarter) quantityPeriod += 0.25;
                date = date.AddMinutes(quantityPeriod);

                fixedMinute = true;
            }

            if (isHour && fixedQuantityPeriod && fixedHour == false)
            {
                date = DateTime.Now;
                if (isHalf)
                {
                    quantityPeriod += 0.5;
                    fixedMinute = true;
                }

                if (isQuarter)
                {
                    quantityPeriod += 0.25;
                    fixedMinute = true;
                }

                date = date.AddHours(quantityPeriod);

                fixedHour = true;
            }

            if (isWeek && fixedQuantityPeriod && fixedWeek == false)
            {
                date = DateTime.Now;
                if (isHalf) quantityPeriod += 0.5;
                if (isQuarter) quantityPeriod += 0.25;
                date = date.AddDays(7d * quantityPeriod);

                fixedWeek = true;
            }
        }

        private void DayOfWeekInWords(ref bool isDayOfWeek, ref bool fixedDay, ref bool isQuantity, ref bool fixedQuantity, ref double quantity, ref DateTime date, string word, int dayOfWeek, string culture, bool isHalf, bool isQuarter, bool isAM, bool isPM)
        {
            if (utility.GetEnumItems(typeof(DaysOfTheWeekIta)).Contains(word) && fixedDay == false) isDayOfWeek = true;

            if (isDayOfWeek && fixedDay == false)
            {
                try
                {
                    int newDayOfWeek = 0;
                    var cultureInfo = default(CultureInfo);
                    if (culture.ToLower() == "it-it")
                        cultureInfo = new CultureInfo("it-IT");
                    if (culture.ToLower() == "en-us")
                        cultureInfo = new CultureInfo("en-US");

                    if (culture.ToLower() == "it-it")
                        newDayOfWeek = (int)(DaysOfTheWeekIta)Enum.Parse(typeof(DaysOfTheWeekIta), utility.FirstLetterToUpper(word.Trim()));
                    if (culture.ToLower() == "en-us")
                        newDayOfWeek = (int)(DaysOfTheWeekEng)Enum.Parse(typeof(DaysOfTheWeekEng), utility.FirstLetterToUpper(word.Trim()));

                    double countDays = 0;
                    if (newDayOfWeek > dayOfWeek)
                        countDays = newDayOfWeek - dayOfWeek;

                    if (newDayOfWeek < dayOfWeek)
                        countDays = 7 - (dayOfWeek - newDayOfWeek);

                    if (newDayOfWeek == dayOfWeek)
                        countDays = 7;

                    countDays += 7 * quantity;

                    date = DateTime.Parse(date.Day + "/" + date.Month + "/" + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second, new CultureInfo("it-IT"));

                    date = date.AddDays(countDays);

                    fixedDay = true;
                }
                catch (Exception)
                { }
            }

            if (isQuantity && fixedQuantity == false)
            {
                try
                {
                    quantity = double.Parse(word.Trim().ToLower()) - 1;

                    fixedQuantity = true;
                }
                catch (Exception)
                { }
            }

            if (culture.ToLower() == "it-it" && utility.GetEnumItems(typeof(TemporalComplementIta)).Contains(word.Trim().ToLower()) && fixedDay == false && fixedQuantity == false) isQuantity = true;
            if (culture.ToLower() == "en-us" && utility.GetEnumItems(typeof(TemporalComplementEng)).Contains(word.Trim().ToLower()) && fixedDay == false && fixedQuantity == false) isQuantity = true;
        }

        private void DateTimeInNumbers(ref bool isNumericDay, ref bool fixedDay, ref bool isNumericMonth, ref bool fixedMonth, ref bool isNumericYear, ref bool fixedYear, ref bool isNumericHour, ref bool fixedHour, ref bool isNumericMinute, ref bool fixedMinute, ref DateTime date, string word, string culture, bool isHalf, bool isQuarter, bool isAM, bool isPM)
        {
            var cultureInfo = default(CultureInfo);
            if (culture.ToLower() == "it-it")
                cultureInfo = new CultureInfo("it-IT");
            if (culture.ToLower() == "en-us")
                cultureInfo = new CultureInfo("en-US");

            var _quantity = 0;

            if (culture.ToLower() == "it-it")
            {
                if (word.Trim().ToLower() == DaysIta.Oggi.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 0;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }
                if (word.Trim().ToLower() == DaysIta.Domani.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 1;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }
                if (word.Trim().ToLower() == DaysIta.Dopodomani.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 2;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }

                if (word.Trim().ToLower() == TimesIta.Mezzogiorno.ToString().ToLower() && fixedHour == false && fixedMinute == false)
                {
                    date = DateTime.Parse(date.Day + "/" + date.Month + "/" + date.Year + " " + "12" + ":" + "00" + ":" + "00", new CultureInfo("it-IT"));
                    fixedHour = true;
                    fixedMinute = true;
                }

                if (word.Trim().ToLower() == TimesIta.Mezzanotte.ToString().ToLower() && fixedHour == false && fixedMinute == false)
                {
                    date = DateTime.Parse(date.Day + "/" + date.Month + "/" + date.Year + " " + "00" + ":" + "00" + ":" + "00", new CultureInfo("it-IT"));
                    fixedHour = true;
                    fixedMinute = true;
                }
            }

            if (culture.ToLower() == "en-us")
            {
                if (word.Trim().ToLower() == DaysEng.Today.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 0;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }
                if (word.Trim().ToLower() == DaysEng.Tomorrow.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 1;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }
                if (word.Trim().ToLower() == DaysEng.DayAfterTomorrow.ToString().ToLower() && fixedDay == false)
                {
                    _quantity = 2;
                    date = date.AddDays(_quantity);
                    fixedDay = true;
                }

                if ((word.Trim().ToLower() == TimesEng.Midday.ToString().ToLower() || word.Trim().ToLower() == TimesEng.Noon.ToString().ToLower()) && fixedHour == false && fixedMinute == false)
                {
                    date = DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " " + "12" + ":" + "00" + ":" + "00 pm", new CultureInfo("en-US"));
                    fixedHour = true;
                    fixedMinute = true;
                }

                if (word.Trim().ToLower() == TimesEng.Midnight.ToString().ToLower() && fixedHour == false && fixedMinute == false)
                {
                    date = DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " " + "12" + ":" + "00" + ":" + "00 am", new CultureInfo("en-US"));
                    fixedHour = true;
                    fixedMinute = true;
                }
            }

            if (isNumericDay && fixedDay == false)
            {
                try
                {
                    var day = int.Parse(word);
                    date = DateTime.Parse(day + "/" + date.Month + "/" + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second, new CultureInfo("it-IT"));
                    fixedDay = true;
                }
                catch (Exception)
                { }
            }

            if (isNumericMonth && fixedMonth == false)
            {
                try
                {
                    var month = int.Parse(word);
                    date = DateTime.Parse(date.Day + "/" + month + "/" + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second, new CultureInfo("it-IT"));
                    fixedMonth = true;
                }
                catch (Exception)
                { }
            }

            if (isNumericYear && fixedYear == false)
            {
                try
                {
                    var year = int.Parse(word);
                    date = DateTime.Parse(date.Day + "/" + date.Month + "/" + year + " " + date.Hour + ":" + date.Minute + ":" + date.Second, new CultureInfo("it-IT"));
                    fixedYear = true;
                }
                catch (Exception)
                { }
            }

            if (isNumericHour && fixedHour == false)
            {
                try
                {
                    var hour = int.Parse(word);
                    var minute = date.Minute;
                    if (isHalf) minute = 30;
                    if (isQuarter) minute = 15;

                    if (culture.ToLower() == "it-it")
                        date = DateTime.Parse(date.Day + "/" + date.Month + "/" + date.Year + " " + hour + ":" + minute + ":" + "00", new CultureInfo("it-IT"));

                    if (isAM || (isAM == false && isPM == false) && culture.ToLower() == "en-us")
                        date = DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " " + hour + ":" + minute + ":" + "00 am", new CultureInfo("en-US"));

                    if (isPM && culture.ToLower() == "en-us")
                        date = DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " " + hour + ":" + minute + ":" + "00 pm", new CultureInfo("en-US"));

                    fixedHour = true;
                }
                catch (Exception)
                { }
            }

            if (isNumericMinute && fixedMinute == false)
            {
                try
                {
                    var minute = 0;
                    if (isHalf)
                        minute = 30;
                    else if (isQuarter)
                        minute = 15;
                    else
                        minute = int.Parse(word);

                    date = DateTime.Parse(date.Day + "/" + date.Month + "/" + date.Year + " " + date.Hour + ":" + minute + ":" + date.Second, new CultureInfo("it-IT"));
                    fixedMinute = true;
                }
                catch (Exception)
                { }
            }

            if (culture.ToLower() == "it-it")
            {
                if (word.Trim().ToLower() == KeywordsIta.Il.ToString().ToLower() && fixedDay == false) isNumericDay = true;
                if (word.Trim().ToLower() == KeywordsIta.Del.ToString().ToLower() && fixedDay == true && fixedMonth == false) isNumericMonth = true;
                if (word.Trim().ToLower() == KeywordsIta.Del.ToString().ToLower() && fixedDay == true && fixedMonth == true && fixedYear == false) isNumericYear = true;
                if (word.Trim().ToLower() == KeywordsIta.Alle.ToString().ToLower() && fixedHour == false) isNumericHour = true;
                if (word.Trim().ToLower() == KeywordsIta.E.ToString().ToLower() && fixedHour == true && fixedMinute == false) isNumericMinute = true;
            }

            if (culture.ToLower() == "en-us")
            {
                if (word.Trim().ToLower() == KeywordsEng.On.ToString().ToLower() && fixedDay == false) isNumericDay = true;
                if (word.Trim().ToLower() == KeywordsEng.Of.ToString().ToLower() && fixedDay == true && fixedMonth == false) isNumericMonth = true;
                if (word.Trim().ToLower() == KeywordsEng.Of.ToString().ToLower() && fixedDay == true && fixedMonth == true && fixedYear == false) isNumericYear = true;
                if (word.Trim().ToLower() == KeywordsEng.At.ToString().ToLower() && fixedHour == false) isNumericHour = true;
                if (word.Trim().ToLower() == KeywordsEng.And.ToString().ToLower() && fixedHour == true && fixedMinute == false) isNumericMinute = true;
            }
        }

        public enum DaysOfTheWeekIta
        {
            Lunedì = 1,
            Martedì = 2,
            Mercoledì = 3,
            Giovedì = 4,
            Venerdì = 5,
            Sabato = 6,
            Domenica = 7
        }

        public enum DaysOfTheWeekEng
        {
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7
        }

        public enum DaysIta
        {
            Oggi = 0,
            Domani = 1,
            Dopodomani = 3
        }

        public enum DaysEng
        {
            Today = 0,
            Tomorrow = 1,
            DayAfterTomorrow = 3
        }

        public enum TimesIta
        {
            Mezzogiorno = 0,
            Mezzanotte = 1,
        }

        public enum TimesEng
        {
            Midday = 0,
            Noon = 1,
            Midnight = 2
        }

        public enum MothsIta
        {
            Gennaio = 1,
            Febbraio = 2,
            Marzo = 3,
            Aprile = 4,
            Maggio = 5,
            Giugno = 6,
            Luglio = 7,
            Agosto = 8,
            Settembre = 9,
            Ottobre = 10,
            Novembre = 11,
            Dicembre = 12
        }

        public enum MothsEng
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum TimeDefinitionsIta
        {
            Anno = 1,
            Anni = 2,
            Mese = 3,
            Mesi = 4,
            Settimana = 5,
            Settimane = 6,
            Giorno = 7,
            Giorni = 8,
            Ora = 9,
            Ore = 10,
            Minuto = 11,
            Minuti = 12,
            Secondo = 13,
            Secondi = 14,
        }

        public enum TimeDefinitionsEng
        {
            Year = 1,
            Years = 2,
            Month = 3,
            Months = 4,
            Week = 5,
            Weeks = 6,
            Day = 7,
            Days = 8,
            Hour = 9,
            Hours = 10,
            Minute = 11,
            Minutes = 12,
            Second = 13,
            Seconds = 14,
        }

        public enum TemporalComplementIta
        {
            Fra = 0,
            Tra = 1
        }

        public enum TemporalComplementEng
        {
            In = 0
        }

        public enum QuantityInWordsIta
        {
            Un = 1,
            Una = 2,
            Mezzo = 3,
            Mezza = 4,
            StessOra = 5,
            StessaOra = 6,
            QuestOra = 7,
            QuestaOra = 8,
            Quarto = 9,
            Quarti = 10
        }

        public enum QuantityInWordsEng
        {
            One = 1,
            Half = 2,
            SameTime = 3,
            SameHour = 4,
            ThisHour = 5,
            Quarter = 6
        }

        public enum KeywordsIta
        {
            Il = 0,
            Del = 1,
            Alle = 2,
            E = 3
        }

        public enum KeywordsEng
        {
            On = 0,
            Of = 1,
            At = 2,
            And = 3
        }
    }
}