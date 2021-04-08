using TimeTrack.Web.Service.Tools.V1;
using Xunit;

namespace TimeTrack.Web.Service.UnitTest
{
    public class TimeSpanConvertTest
    {
        [Fact]
        public void HourMinuteFormatTest()
        {
            var time = TimeSpanConverter.ToTimeSpan("02:00");
            
            Assert.Equal(2, time.Hours);
            Assert.Equal(0, time.Minutes);
        }
    }
}