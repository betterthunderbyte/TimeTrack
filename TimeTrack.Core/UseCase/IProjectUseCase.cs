using System.Threading.Tasks;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IProjectUseCase
    {
        public Task<UseCaseResult<ProjectEntity>> GetAllAsync();
        public Task<UseCaseResult<ProjectEntity>> GetSingleAsync(int id);
        public Task<UseCaseResult<ProjectEntity>> UpdateSingleAsync(int id, ProjectEntity projectEntity);
        public Task<UseCaseResult<ProjectEntity>> CreateSingleAsync(ProjectEntity projectEntity);
        public Task<UseCaseResult<ProjectEntity>> DeleteSingleAsync(int id);
    }
}