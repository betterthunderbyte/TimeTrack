using System;
using System.Text.Json.Serialization;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class ActivityDataTransfer : IUseCaseConverter<ActivityEntity>
    {
        public int Id { get; set; }

        public DateTimeDataTransfer Begin { get; set; }
        
        public TimeDataTransfer Duration { get; set; }

        public int ActivityTypeFk { get; set; }
        
        public int ProjectFk { get; set; }
        
        public int CustomerFk { get; set; }
        
        public int OwnerFk { get; set; }
        
        public void To(out ActivityEntity output)
        {
            Duration.To(out var duration);
            Begin.To(out var dateTimeOffset);
            output = new ActivityEntity()
            {
                Id = Id,
                Begin = dateTimeOffset,
                Duration = duration,
                ActivityTypeFk = ActivityTypeFk,
                CustomerFk = CustomerFk,
                ProjectFk = ProjectFk,
                OwnerFk = OwnerFk
            };
        }

        public void From(ActivityEntity input)
        {
            var duration = new TimeDataTransfer();
            duration.From(input.Duration);
            
            var begin = new DateTimeDataTransfer();
            begin.From(input.Begin);
            
            Id = input.Id;
            Begin = begin;
            Duration = duration;
            ActivityTypeFk = input.ActivityTypeFk;
            ProjectFk = input.ProjectFk;
            CustomerFk = input.CustomerFk;
            OwnerFk = input.OwnerFk;
        }
    }
}