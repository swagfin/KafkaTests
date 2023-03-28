using System;

namespace Consumer.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly string _defaultFallbackTime;

        /// <summary>
        /// Converts the Specified UTC DateTime to LocalTime
        /// </summary>
        /// <param name="dateTimeUTC"></param>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static DateTime ConvertUtcToLocalTime(this DateTime dateTimeUTC, string timezone)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                dateTimeUTC = SetKind(dateTimeUTC);
                return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUTC, tz);
            }
            catch { }
            return dateTimeUTC;
        }
        /// <summary>
        /// Converts the Specified UTC DateTime (Nullable) to LocalTime
        /// </summary>
        /// <param name="dateTimeUTC"></param>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static DateTime? ConvertUtcToLocalTime(this DateTime? dateTimeUTC, string timezone)
        {
            if (!dateTimeUTC.HasValue)
                return null;
            return ConvertUtcToLocalTime(dateTimeUTC.Value, timezone);
        }

        /// <summary>
        /// Converts the Specified Local DateTime to UTC
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static DateTime ConvertLocalTimeToUTC(this DateTime localDateTime, string timezone)
        {
            try
            {
                timezone = string.IsNullOrWhiteSpace(timezone) ? _defaultFallbackTime : timezone;
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                localDateTime = SetKind(localDateTime);
                return TimeZoneInfo.ConvertTimeToUtc(localDateTime, tz);
            }
            catch { }
            return localDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts the Specified Local DateTime (Nullable) to UTC
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static DateTime? ConvertLocalTimeToUTC(this DateTime? localDateTime, string timezone)
        {
            if (!localDateTime.HasValue)
                return null;
            return ConvertLocalTimeToUTC(localDateTime.Value, timezone);
        }
        private static DateTime SetKind(DateTime date) => DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
    }
}
