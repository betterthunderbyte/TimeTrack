using System;
using System.Xml.Serialization;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(DateTimeDataTransfer))]
    public class DateTimeDataTransfer : IUseCaseConverter<DateTimeOffset>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        
        public TimeDataTransfer Time { get; set; }

        public long Ticks { get; set; }

        public string DefaultFormat
        {
            get{ return $"{Year.ToString()}-{Month.ToString().PadLeft(2, '0')}-{Day.ToString().PadLeft(2, '0')}";}
        }
        
        public void To(out DateTimeOffset output)
        {
            output = new DateTimeOffset(
                Year, 
                Month, 
                Day, 
                Time.Hours, 
                Time.Minutes, 
                Time.Seconds, 
                TimeSpan.Zero
                );
        }

        public void From(DateTimeOffset input)
        {
            Year = input.Year;
            Month = input.Month;
            Day = input.Day;
            Time = new TimeDataTransfer()
            {
                Hours = input.Hour,
                Minutes = input.Minute,
                Seconds = input.Second,
            };
            Ticks = (long)input
                .ToUniversalTime()
                .Subtract(DateTime.UnixEpoch)
                .TotalSeconds;;
        }
    }
}