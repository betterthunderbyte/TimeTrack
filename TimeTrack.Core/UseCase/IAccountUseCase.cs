using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IAccountUseCase
    {
        public Task<UseCaseResult<MemberEntity>> ValidateLoginAsync(LoginDataTransfer loginDataTransfer);
        public Task<UseCaseResult<MemberEntity>> LoginAsync(LoginDataTransfer loginDataTransfer);
    }
}