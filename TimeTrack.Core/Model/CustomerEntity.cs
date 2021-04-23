using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.Model
{
    public class CustomerEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ActivityEntity> Activities { get; set; }
    }
}
