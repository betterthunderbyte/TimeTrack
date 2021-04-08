using System;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class TimeDataTransfer : IUseCaseConverter<TimeSpan>
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public double AsHours
        {
            get
            {
                return Hours + (Minutes * 0.0166666666666667) + (Seconds * 0.0166666666666667 * 0.0166666666666667);
            }
        }

        public string DefaultFormatHHMMSS
        {
            get{ return $"{Hours.ToString().PadLeft(2, '0')}:{Minutes.ToString().PadLeft(2, '0')}:{Seconds.ToString().PadLeft(2, '0')}";}
        }

        public string DefaultFormatHHMM
        {
            get{ return $"{Hours.ToString().PadLeft(2, '0')}:{Minutes.ToString().PadLeft(2, '0')}";}
        }

        
        public void To(out TimeSpan output)
        {
            output = new TimeSpan(Hours, Minutes, Seconds);
        }

        public void From(TimeSpan input)
        {
            Hours = input.Hours;
            Minutes = input.Minutes;
            Seconds = input.Seconds;
        }
    }
}