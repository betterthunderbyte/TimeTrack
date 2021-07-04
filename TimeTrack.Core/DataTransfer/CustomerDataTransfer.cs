using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(CustomerDataTransfer))]
    public class CustomerDataTransfer : IUseCaseConverter<CustomerEntity>
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public void From(CustomerEntity input)
        {
            Id = input.Id;
            Name = input.Name;
        }

        public void To(out CustomerEntity output)
        {
            output = new CustomerEntity() { Id = Id, Name = Name };
        }
    }
}
