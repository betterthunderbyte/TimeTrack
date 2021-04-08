using System.Threading.Tasks;
using TimeTrack.Models.V1;

namespace TimeTrack.Core.UseCase
{
    public interface IAccountUseCase
    {
        public Task<UseCaseResult<MemberEntity>> ValidateLoginAsync(string mail, string password);
    }
}