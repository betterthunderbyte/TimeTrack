using System.Threading.Tasks;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface ICustomerUseCase
    {
        public Task<UseCaseResult<CustomerEntity>> GetAllAsync();

        public Task<UseCaseResult<CustomerEntity>> GetSingleAsync(int id);

        public Task<UseCaseResult<CustomerEntity>> CreateSingleAsync(CustomerEntity customerEntity);

        public Task<UseCaseResult<CustomerEntity>> DeleteSingleAsync(int id);
        public Task<UseCaseResult<CustomerEntity>> UpdateSingleAsync(int id, CustomerEntity customer);
    }
}