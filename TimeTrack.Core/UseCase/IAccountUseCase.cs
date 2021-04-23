using System.Threading.Tasks;
using TimeTrack.Core.Model;

namespace TimeTrack.Core.UseCase
{
    public interface IAccountUseCase
    {
        public Task<UseCaseResult<MemberEntity>> ValidateLoginAsync(string mail, string password);
    }
}