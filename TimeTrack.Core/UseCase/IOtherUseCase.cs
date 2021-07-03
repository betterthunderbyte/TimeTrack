using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer;

namespace TimeTrack.Core.UseCase
{
    public interface IOtherUseCase
    {
        public Task<UseCaseResult<FullDataTransfer>> GetFullFromUserAsync(int ownerId);
    }
}