using System.Threading.Tasks;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IMemberUseCase
    {
        public Task<UseCaseResult<MemberEntity>> GetAllAsync();
        public Task<UseCaseResult<MemberEntity>> GetSingleAsync(int id);
        public Task<UseCaseResult<MemberEntity>> SetPassword(int id, string password);
        public Task<UseCaseResult<MemberEntity>> PutSingleAsync(MemberEntity member);
        public Task<UseCaseResult<MemberEntity>> DeleteSingleAsync(int id);
        public Task<UseCaseResult<MemberEntity>> UpdateSingleAsync(int id, MemberEntity member);
    }
}