using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrack.Models.V1
{
    public class ActivityEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset Begin { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public int ActivityTypeFk { get; set; }
        public virtual ActivityTypeEntity ActivityType { get; set; }
        
        public int ProjectFk { get; set; }
        public virtual ProjectEntity Project { get; set; }

        public int CustomerFk { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        
        public int OwnerFk { get; set; }
        public virtual MemberEntity Owner { get; set; }
    }
}
