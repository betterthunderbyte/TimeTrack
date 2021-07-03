using System.Threading.Tasks;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IActivityTypeUseCase
    {
        public Task<UseCaseResult<ActivityTypeEntity>> GetAllAsync();
        
        public Task<UseCaseResult<ActivityTypeEntity>> GetSingleAsync(int id);
        public Task<UseCaseResult<ActivityTypeEntity>> CreateSingleAsync(ActivityTypeEntity activityTypeEntity);

        public Task<UseCaseResult<ActivityTypeEntity>> UpdateSingleAsync(int id,
            ActivityTypeEntity activityTypeEntity);

        public Task<UseCaseResult<ActivityTypeEntity>> DeleteSingleAsync(int id);
    }
}