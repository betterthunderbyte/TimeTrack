using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TimeTrack.Core;
using TimeTrack.Core.Configuration;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Core.Model;
using TimeTrack.Core.UseCase;

namespace TimeTrack.UseCase
{
    public class AccountUseCase : IAccountUseCase
    {
        ITimeTrackDbContext _context;
        private JsonWebTokenConfiguration _configuration;
        
        public AccountUseCase(ITimeTrackDbContext timeTrackTimeTrackDbContext, IOptions<JsonWebTokenConfiguration> configuration)
        {
            _context = timeTrackTimeTrackDbContext;
            _configuration = configuration.Value;
        }

        public async Task<UseCaseResult<MemberEntity>> ValidateLoginAsync(LoginDataTransfer loginDataTransfer)
        {
            var validationResult = loginDataTransfer.IsValid();
            if (!validationResult)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage()
                {
                    Message = validationResult.Message
                });
            }
            
            var member = await _context.Members.SingleOrDefaultAsync(
                x => x.Mail == loginDataTransfer.Mail
            );

            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }
            
            if (!member.Active)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }

            if (!member.VerifyPassword(loginDataTransfer.Password))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }
            
            return UseCaseResult<MemberEntity>.Success(member);
        }

        public async Task<UseCaseResult<MemberEntity>> LoginAsync(LoginDataTransfer loginDataTransfer)
        {
            var member = await _context.Members.SingleOrDefaultAsync(
                x => x.Mail == loginDataTransfer.Mail
            );
            
            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(
                    UseCaseResultType.BadRequest,     
                    new { Message = "Die E-Mail oder das Passwort ist falsch!"}
                );
            }

            if (!member.VerifyPassword(loginDataTransfer.Password))
            {
                return UseCaseResult<MemberEntity>.Failure(
                    UseCaseResultType.BadRequest,     
                    new { Message = "Die E-Mail oder das Passwort ist falsch!"}
                );
            }

            return UseCaseResult<MemberEntity>.Success(member);
        }
        
    }
}

