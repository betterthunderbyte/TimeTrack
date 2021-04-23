using System.ComponentModel.DataAnnotations;
using TimeTrack.Core.Model;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer
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
