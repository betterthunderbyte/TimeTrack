using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core;
using TimeTrack.Core.Model;
using TimeTrack.Core.UseCase;

namespace TimeTrack.UseCase
{
    public class CustomerUseCase : ICustomerUseCase
    {
        IDbContext _timeTrackDbContext;

        public CustomerUseCase(IDbContext timeTrackDbContext)
        {
            _timeTrackDbContext = timeTrackDbContext;
        }

        public async Task<UseCaseResult<CustomerEntity>> GetAllAsync()
        {
            var r = await _timeTrackDbContext.Customers.ToListAsync();

            return UseCaseResult<CustomerEntity>.Success(r);
        }
        
        public async Task<UseCaseResult<CustomerEntity>> GetSingleAsync(int id)
        {
            
            var r = await _timeTrackDbContext.Customers.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }
            
            return UseCaseResult<CustomerEntity>.Success(r);
        }

        public async Task<UseCaseResult<CustomerEntity>> CreateSingleAsync(CustomerEntity customerEntity)
        {
            if (customerEntity == null)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Kundendatensatz ist fehlerhaft!"
                });
            }

            if (string.IsNullOrWhiteSpace(customerEntity.Name))
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name ist leer."
                });
            }

            if (customerEntity.Name.Length > 100)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name ist länger als 100 Zeichen."
                });
            }

            customerEntity.Name = customerEntity.Name.Trim();
            
            if (await _timeTrackDbContext.Customers.CountAsync(x => x.Name == customerEntity.Name) == 1)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.Conflict, new
                {
                    Message="Es existiert bereits ein Datensatz mit dem Namen.",
                    Name=customerEntity.Name
                });
            }
            
            customerEntity.Id = 0;
            
            await _timeTrackDbContext.Customers.AddAsync(customerEntity);
            await _timeTrackDbContext.SaveChangesAsync();

            return UseCaseResult<CustomerEntity>.Success(customerEntity);
        }

        public async Task<UseCaseResult<CustomerEntity>> DeleteSingleAsync(int id)
        {
            if (await _timeTrackDbContext.Customers.CountAsync() == 1)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.Conflict, new
                {
                    Message="Der letzte Datensatz kann nicht gelöscht werden!"
                });
            }
            
            var r = await _timeTrackDbContext.Customers.Include(x => x.Activities).SingleOrDefaultAsync(x => x.Id == id);
            if(r == null)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.NotFound, null);
            }

            if (r.Activities != null && r.Activities.Count > 0)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, null);
            }

            _timeTrackDbContext.Customers.Remove(r);
            await _timeTrackDbContext.SaveChangesAsync();

            return UseCaseResult<CustomerEntity>.Success(r);
        }

        public async Task<UseCaseResult<CustomerEntity>> UpdateSingleAsync(int id, CustomerEntity customer)
        {
            if (customer == null)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Kundendatensatz ist fehlerhaft!"
                });
            }
            
            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name fehlt."
                });
            }
            
            if (customer.Name.Length > 100)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name ist länger als 100 Zeichen."
                });
            }
            
            customer.Name = customer.Name.Trim();

            if (await _timeTrackDbContext.Customers.CountAsync(x => x.Name == customer.Name) == 1)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.Conflict, new
                {
                    Message="Der Datensatz mit dem Namen existiert bereits."
                });
            }

            var r = await _timeTrackDbContext.Customers.SingleOrDefaultAsync(x => x.Id == id);
            if (r == null)
            {
                return UseCaseResult<CustomerEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }

            r.Name = customer.Name;
            await _timeTrackDbContext.SaveChangesAsync();

            return UseCaseResult<CustomerEntity>.Success(r);
        }
    }
}
