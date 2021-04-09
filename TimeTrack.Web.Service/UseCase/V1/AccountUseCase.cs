using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeTrack.Core;
using TimeTrack.Models.V1;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Db;
using TimeTrack.Web.Service.Options;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.UseCase.V1
{
    public class AccountUseCase
    {
        TimeTrackDbContext _context;
        private JsonWebTokenConfiguration _configuration;
        
        public AccountUseCase(TimeTrackDbContext timeTrackDbContext, IOptions<JsonWebTokenConfiguration> configuration)
        {
            _context = timeTrackDbContext;
            _configuration = configuration.Value;
        }

        public async Task<UseCaseResult<MemberEntity>> ValidateLoginAsync(string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(mail))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {Message="E-Mail nicht vorhanden!"});
            }
            
            if (string.IsNullOrWhiteSpace(password))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {Message="Passwort nicht vorhanden!"});
            }
            
            var member = await _context.Members.SingleOrDefaultAsync(
                x => x.Mail == mail
            );

            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }
            
            if (!member.Active)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }

            if (!member.VerifyPassword(password))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new ErrorMessage {});
            }
            
            return UseCaseResult<MemberEntity>.Success(member);
        }

        public async Task<UseCaseResult<NewTokenDataTransfer>> LoginAsync(LoginDataTransfer loginDataTransfer)
        {
            var member = await _context.Members.SingleOrDefaultAsync(
                x => x.Mail == loginDataTransfer.Mail
            );
            
            if (member == null)
            {
                return UseCaseResult<NewTokenDataTransfer>.Failure(
                    UseCaseResultType.BadRequest,     
                    new { Message = "Die E-Mail oder das Passwort ist falsch!"}
                );
            }

            if (!member.VerifyPassword(loginDataTransfer.Password))
            {
                return UseCaseResult<NewTokenDataTransfer>.Failure(
                    UseCaseResultType.BadRequest,     
                    new { Message = "Die E-Mail oder das Passwort ist falsch!"}
                );
            }
            
            string role = "none";
            
            switch (member.Role)
            {
                case MemberEntity.MemberRole.Admin:
                    role = "admin";
                    break;
                case MemberEntity.MemberRole.Moderator:
                    role = "moderator";
                    break;
                case MemberEntity.MemberRole.User:
                    role = "user";
                    break;
                default:
                    break;
            }
            
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration.Secret)
            );
            var credentials = new SigningCredentials(
                securityKey, 
                SecurityAlgorithms.HmacSha256
            );
            var token = new JwtSecurityToken(
                _configuration.Issuer, 
                _configuration.Audience, 
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Surname, member.Surname),
                    new Claim(ClaimTypes.GivenName, member.GivenName),
                    new Claim(ClaimTypes.Email, member.Mail),
                }, 
                null, 
                DateTime.Now.AddSeconds(_configuration.AccessTokenExpiration), 
                credentials);
            
            return UseCaseResult<NewTokenDataTransfer>.Success(new NewTokenDataTransfer()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        
    }
}

