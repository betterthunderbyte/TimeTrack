using System;
using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IActivityUseCase
    {
        public Task<UseCaseResult<ActivityEntity>> GetAllFromUserAsync(int userId);

        public Task<UseCaseResult<WeekDurationDataTransfer>> GetDurationFromWeekInDaysFromOwner(int userId,
            DateTimeOffset dateTime);

        public Task<UseCaseResult<ActivityEntity>> GetAllAsync();
        public Task<UseCaseResult<ActivityEntity>> GetSingleAsync(int id);
        public Task<UseCaseResult<ActivityEntity>> CreateSingleAsync(ActivityEntity activityEntity);
        public Task<UseCaseResult<ActivityEntity>> DeleteSingleAsync(int id);
        public Task<UseCaseResult<ActivityEntity>> DeleteSingleFromUserAsync(int userId, int id);

        public Task<UseCaseResult<ActivityEntity>> UpdateSingleFromUserAsync(int userId, int id,
            ActivityEntity activityEntity);

        public Task<UseCaseResult<ActivityEntity>> UpdateSingleAsync(int id, ActivityEntity activityEntity);
    }
}