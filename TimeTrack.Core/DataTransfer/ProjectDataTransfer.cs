using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(ProjectDataTransfer))]
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
