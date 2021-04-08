using System;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class WeekDurationDataTransfer
    {
        public TimeDataTransfer Monday { get; set; }
        public TimeDataTransfer Tuesday { get; set; }
        public TimeDataTransfer Wednesday { get; set; }
        public TimeDataTransfer Thursday { get; set; }
        public TimeDataTransfer Friday { get; set; }
        public TimeDataTransfer Saturday { get; set; }
        public TimeDataTransfer Sunday { get; set; }
    }
}