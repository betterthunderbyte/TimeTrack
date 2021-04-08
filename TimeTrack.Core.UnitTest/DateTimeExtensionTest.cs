using System;
using TimeTrack.Web.Service.Tools.V1;
using Xunit;

namespace TimeTrack.Core.UnitTest
{
    public class DateTimeExtensionTest
    {
        
        
        [Fact]
        public void GetMondayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero).GetMondayInThisWeek();
            Assert.Equal(DayOfWeek.Monday, dateTimeOffset.DayOfWeek);
            Assert.Equal(28, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetMondayInThisWeek();
            Assert.Equal(DayOfWeek.Monday, dateTimeOffset.DayOfWeek);
            Assert.Equal(28, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetMondayInThisWeek();
            Assert.Equal(DayOfWeek.Monday, dateTimeOffset.DayOfWeek);
            Assert.Equal(28, dateTimeOffset.Day);
        }
        
        [Fact]
        public void GetTuesdayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetTuesdayInThisWeek();
            Assert.Equal(DayOfWeek.Tuesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(29, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetTuesdayInThisWeek();
            Assert.Equal(DayOfWeek.Tuesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(29, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetTuesdayInThisWeek();
            Assert.Equal(DayOfWeek.Tuesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(29, dateTimeOffset.Day);
        }

                
        [Fact]
        public void GetWednesdayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetWednesdayInThisWeek();
            Assert.Equal(DayOfWeek.Wednesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(30, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetWednesdayInThisWeek();
            Assert.Equal(DayOfWeek.Wednesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(30, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetWednesdayInThisWeek();
            Assert.Equal(DayOfWeek.Wednesday, dateTimeOffset.DayOfWeek);
            Assert.Equal(30, dateTimeOffset.Day);
        }

                        
        [Fact]
        public void GetThursdayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetThursdayInThisWeek();
            Assert.Equal(DayOfWeek.Thursday, dateTimeOffset.DayOfWeek);
            Assert.Equal(31, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetThursdayInThisWeek();
            Assert.Equal(DayOfWeek.Thursday, dateTimeOffset.DayOfWeek);
            Assert.Equal(31, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetThursdayInThisWeek();
            Assert.Equal(DayOfWeek.Thursday, dateTimeOffset.DayOfWeek);
            Assert.Equal(31, dateTimeOffset.Day);
        }
        
        [Fact]
        public void GetFridayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetFridayInThisWeek();
            Assert.Equal(DayOfWeek.Friday, dateTimeOffset.DayOfWeek);
            Assert.Equal(1, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetFridayInThisWeek();
            Assert.Equal(DayOfWeek.Friday, dateTimeOffset.DayOfWeek);
            Assert.Equal(1, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetFridayInThisWeek();
            Assert.Equal(DayOfWeek.Friday, dateTimeOffset.DayOfWeek);
            Assert.Equal(1, dateTimeOffset.Day);
        }

        [Fact]
        public void GetSaturdayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetSaturdayInThisWeek();
            Assert.Equal(DayOfWeek.Saturday, dateTimeOffset.DayOfWeek);
            Assert.Equal(2, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 03, 12, 0, 0, TimeSpan.Zero)
                .GetSaturdayInThisWeek();
            Assert.Equal(DayOfWeek.Saturday, dateTimeOffset.DayOfWeek);
            Assert.Equal(2, dateTimeOffset.Day);
            
            dateTimeOffset = new DateTimeOffset(2021, 01, 02, 12, 0, 0, TimeSpan.Zero)
                .GetSaturdayInThisWeek();
            Assert.Equal(DayOfWeek.Saturday, dateTimeOffset.DayOfWeek);
            Assert.Equal(2, dateTimeOffset.Day);
        }
        
        [Fact]
        public void GetSundayTest()
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(2021, 01, 01, 12, 0, 0, TimeSpan.Zero)
                .GetSundayInThisWeek();
            Assert.Equal(DayOfWeek.Sunday, dateTimeOffset.DayOfWeek);
            Assert.Equal(3, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 04, 12, 0, 0, TimeSpan.Zero)
                .GetSundayInThisWeek();
            Assert.Equal(DayOfWeek.Sunday, dateTimeOffset.DayOfWeek);
            Assert.Equal(10, dateTimeOffset.Day);
            dateTimeOffset = new DateTimeOffset(2021, 01, 06, 12, 0, 0, TimeSpan.Zero)
                .GetSundayInThisWeek();
            Assert.Equal(DayOfWeek.Sunday, dateTimeOffset.DayOfWeek);
            Assert.Equal(10, dateTimeOffset.Day);
        }
    }
}