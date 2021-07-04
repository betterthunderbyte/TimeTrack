using System.Xml.Serialization;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(WeekDurationDataTransfer))]
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