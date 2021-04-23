using System.ComponentModel.DataAnnotations;
using TimeTrack.Core.Model;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer
{
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
