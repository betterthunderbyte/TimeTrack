using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.Model
{
    public class ActivityTypeEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<ActivityEntity> Activities { get; set; }

    }
}
