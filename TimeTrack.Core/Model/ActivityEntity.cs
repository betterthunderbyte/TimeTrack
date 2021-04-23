using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.Model
{
    public class ActivityEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset Begin { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public int ActivityTypeFk { get; set; }
        public ActivityTypeEntity ActivityType { get; set; }
        
        public int ProjectFk { get; set; }
        public ProjectEntity Project { get; set; }

        public int CustomerFk { get; set; }
        public CustomerEntity Customer { get; set; }
        
        public int OwnerFk { get; set; }
        public MemberEntity Owner { get; set; }
    }
}
