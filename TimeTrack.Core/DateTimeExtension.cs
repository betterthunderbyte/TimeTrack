using System;

namespace TimeTrack.Core
{
    public static class DateTimeExtension
    {
        public static DateTimeOffset GetStart(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(
                dateTimeOffset.Year,
                dateTimeOffset.Month,
                dateTimeOffset.Day,
                0,
                0,
                0, 
                dateTimeOffset.Offset);
        }
        
        public static DateTimeOffset GetEnd(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(
                dateTimeOffset.Year,
                dateTimeOffset.Month,
                dateTimeOffset.Day,
                23,
                59,
                59, 
                dateTimeOffset.Offset);
        }
        
        /*
         *  Monday = 0,
         *  Tuesday = 1,
         *  Wednesday = 2,
         *  Thursday = 3,
         *  Friday = 4,
         *  Saturday = 5,
         *  Sunday = 6,
         */
        
        
        public static DateTimeOffset GetMondayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            if (dayOfWeek == 0)
            {
                return dateTimeOffset;
            }
            else
            {
                return dateTimeOffset.AddDays(-dayOfWeek);
            }
        }
        
        public static DateTimeOffset GetTuesdayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            
            if (dayOfWeek == 1)
            {
                return dateTimeOffset;
            }
            else if (dayOfWeek < 1)
            {
                return dateTimeOffset.AddDays(1);
            }
            else
            {
                return dateTimeOffset.AddDays(-(dayOfWeek - 1));
            }
        }
        
        public static DateTimeOffset GetWednesdayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            
            if (dayOfWeek == 2)
            {
                return dateTimeOffset;
            }
            else if (dayOfWeek < 2)
            {
                return dateTimeOffset.AddDays(2 - dayOfWeek);
            }
            else
            {
                return dateTimeOffset.AddDays(-(dayOfWeek - 2));
            }
        }
        
        public static DateTimeOffset GetThursdayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }

            if (dayOfWeek == 3)
            {
                return dateTimeOffset;
            }
            else if (dayOfWeek < 3)
            {
                return dateTimeOffset.AddDays(3 - dayOfWeek);
            }
            else
            {
                return dateTimeOffset.AddDays(-(dayOfWeek - 3));
            }
        }
        
        public static DateTimeOffset GetFridayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
           var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            
            if (dayOfWeek == 4)
            {
                return dateTimeOffset;
            }
            else if(dayOfWeek < 4)
            {
                return dateTimeOffset.AddDays(4 - dayOfWeek); 
            }
            else
            {
                return dateTimeOffset.AddDays(-Math.Abs(4 - dayOfWeek)); 
            }
        }
        
        public static DateTimeOffset GetSaturdayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            
            if (dayOfWeek == 5)
            {
                return dateTimeOffset;
            }
            else if (dayOfWeek < 5)
            {
                return dateTimeOffset.AddDays(5 - dayOfWeek);
            }
            else
            {
                return dateTimeOffset.AddDays(-(dayOfWeek - 5));
            }
        }
        
        public static DateTimeOffset GetSundayInThisWeek(this DateTimeOffset dateTimeOffset)
        {
            var dayOfWeek = (int) dateTimeOffset.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = 6;
            }
            
            
            if (dayOfWeek == 6)
            {
                return dateTimeOffset;
            }
            
            return dateTimeOffset.AddDays(6 - dayOfWeek);
        }
    }
}