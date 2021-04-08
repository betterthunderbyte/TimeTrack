using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrack.Models.V1
{
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<ActivityEntity> Activities { get; set; }
    }
}
