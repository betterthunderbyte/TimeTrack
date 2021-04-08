using System;
using System.Globalization;

namespace TimeTrack.Web.Service.Tools.V1
{
    public static class TimeSpanConverter
    {
        public static TimeSpan ToTimeSpan(string input)
        {
            var time = new TimeSpan();

            if (string.IsNullOrWhiteSpace(input))
            {
                return time;
            }
            
            if(TimeSpan.TryParseExact(input, @"dd\:hh\:mm\:ss", CultureInfo.CurrentCulture, out time))
            {
                return time;
            }
            
            if(TimeSpan.TryParseExact(input, @"hh\:mm\:ss", CultureInfo.CurrentCulture, out time))
            {
                return time;
            }

            if(TimeSpan.TryParseExact(input, @"hh\:mm", CultureInfo.CurrentCulture, out time))
            {
                return time;
            }
            
            return time;
            
        }
    }
}