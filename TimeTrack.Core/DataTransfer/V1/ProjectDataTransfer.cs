using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class ProjectDataTransfer : IUseCaseConverter<ProjectEntity>
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public void From(ProjectEntity input)
        {
            Id = input.Id;
            Name = input.Name;
        }

        public void To(out ProjectEntity output)
        {
            output = new ProjectEntity { Id = Id, Name = Name };
        }
    }
}
