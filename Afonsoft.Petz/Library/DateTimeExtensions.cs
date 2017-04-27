using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Afonsoft.Petz.Library
{
    /// <summary>
    /// Classe com extenção para DATETIME 
    /// AFONSO DUTRA NOGUEIRA FILHO
    /// afonsoft@gmail.com
    /// 03/11/2016
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// ForEach by Day 
        /// </summary>
        /// <param name="from">This date</param>
        /// <param name="thru">Data final</param>
        /// <returns>Next Date</returns>
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        /// business days - not Saturday or not Sunday
        /// </summary>
        public static bool IsWorkingDay(this DateTime from)
        {
            return from.DayOfWeek != DayOfWeek.Saturday
                && from.DayOfWeek != DayOfWeek.Sunday
                && !IsHoliday(from);
        }

        /// <summary>
        /// Lista de feriados nacionais brasileiros (PT-BR)
        /// </summary>
        /// <returns>Array of Holiday</returns>
        public static DateTime[] Holidays(this DateTime from)
        {
            List<DateTime> arrayOfDate = new List<DateTime>();
            int year = from.Year;
            arrayOfDate.Add(DateTime.ParseExact("01/01/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("21/04/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("01/05/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("07/09/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("12/10/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("02/11/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("15/11/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            arrayOfDate.Add(DateTime.ParseExact("25/12/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            DateTime easterDay = CalculateEaster(year);
            arrayOfDate.Add(easterDay); //Domingo de Pascoa (Easter Day)
            arrayOfDate.Add(easterDay.AddDays(-49));
            arrayOfDate.Add(easterDay.AddDays(-48));
            arrayOfDate.Add(easterDay.AddDays(-47)); //The Carnival falls always 47 days before the Easter. 
            arrayOfDate.Add(easterDay.AddDays(-2));
            arrayOfDate.Add(easterDay.AddDays(+60)); //The Corpus Christi falls always 60 days after the Easter.
            return arrayOfDate.ToArray();
        }

        /// <summary>
        /// National Holidays 
        /// </summary>
        public static bool IsHoliday(this DateTime from)
        {
            return Holidays(from).Where(x => x.Year == from.Year && x.Month == from.Month && x.Day == from.Day).Any();
        }

        #region CalculateEaster 
        /// <summary>
        /// FUNÇÃO PARA CALCULAR A DATA DO DOMINGO DE PASCOA
        /// http://www.inf.ufrgs.br/~cabral/Pascoa.html
        /// </summary>
        /// <param name="year">ANO QUALQUER</param>
        /// <returns>DateTime</returns>
        private static DateTime CalculateEaster(int year)
        {
            int x = 24;
            int y = 5;

            if (year >= 1582 && year <= 1699)
            {
                x = 22;
                y = 2;
            }
            else if (year >= 1700 && year <= 1799)
            {
                x = 23;
                y = 3;
            }
            else if (year >= 1800 && year <= 1899)
            {
                x = 24;
                y = 4;
            }
            else if (year >= 1900 && year <= 2099)
            {
                x = 24;
                y = 5;
            }
            else if (year >= 2100 && year <= 2199)
            {
                x = 24;
                y = 6;
            }
            else if (year >= 2200 && year <= 2299)
            {
                x = 25;
                y = 7;
            }

            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int d = (19 * a + x) % 30;
            int e = (2 * b + 4 * c + 6 * d + y) % 7;
            int dia;
            int mes;

            if (d + e > 9)
            {
                dia = (d + e - 9);
                mes = 4;
            }
            else
            {
                dia = (d + e + 22);
                mes = 3;
            }
            return DateTime.Parse(string.Format("{0},{1},{2}", year, mes, dia));
        }
        #endregion

        /// <summary>
        /// Weekend days - Saturday or Sunday
        /// </summary>
        public static bool IsWeekEndDay(this DateTime from)
        {
            return from.DayOfWeek == DayOfWeek.Saturday
                || from.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Return next datetime from a DayOfWeek selected
        /// </summary>
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            /*
                       DayOfWeek
           Domingo         = Sunday    = 0, 
           Segunda-feira   = Monday    = 1, 
           Terça-feira     = Tuesday   = 2, 
           Quarta-feira    = Wednesday = 3, 
           Quinta-feira    = Thursday  = 4, 
           Sexta-feira     = Friday    = 5, 
           Sábado          = Saturday  = 6
           */
            int daysToAdd = ((int)dayOfWeek - (int)from.DayOfWeek + 7) % 7;
            return from.AddDays(daysToAdd);
        }

        /// <summary>
        /// Get Datetime from a workday
        /// </summary>
        public static DateTime AddWorkDays(this DateTime from, int workingDays, params DateTime[] bankHolidays)
        {
            int direction = workingDays < 0 ? -1 : 1;

            DateTime newDate = from;
            // If a working day count of Zero is passed, return the date passed
            if (workingDays == 0)
            {
                newDate = from;
            }
            else
            {
                if (bankHolidays == null)
                    bankHolidays = new[] { DateTime.MinValue };

                while (workingDays != -direction)
                {
                    if (newDate.DayOfWeek != DayOfWeek.Saturday &&
                        newDate.DayOfWeek != DayOfWeek.Sunday &&
                        Array.IndexOf(bankHolidays, newDate) < 0)
                    {
                        workingDays -= direction;
                    }
                    // if the original return date falls on a weekend or holiday, this will take it to the previous / next workday, but the "if" statement keeps it from going a day too far.

                    if (workingDays != -direction)
                    {
                        newDate = newDate.AddDays(direction);
                    }
                }
            }
            return newDate;
        }

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;

            int businessDays = span.Days + 1;

            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;

                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            if (bankHolidays == null)
                bankHolidays = new[] { DateTime.MinValue };

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;

                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
    }
}