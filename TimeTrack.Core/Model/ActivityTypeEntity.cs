using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrack.Models.V1
{
    public class ActivityTypeEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<ActivityEntity> Activities { get; set; }

    }
}
