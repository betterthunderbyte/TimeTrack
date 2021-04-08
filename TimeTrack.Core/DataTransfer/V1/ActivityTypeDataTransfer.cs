using System.ComponentModel.DataAnnotations;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class ActivityTypeDataTransfer : IUseCaseConverter<ActivityTypeEntity>
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        
        public void To(out ActivityTypeEntity output)
        {
            output = new ActivityTypeEntity()
            {
                Id = Id,
                Title = Title,
                Description = Description
            };
        }

        public void From(ActivityTypeEntity input)
        {
            Id = input.Id;
            Title = input.Title;
            Description = input.Description;
            
        }
    }
}