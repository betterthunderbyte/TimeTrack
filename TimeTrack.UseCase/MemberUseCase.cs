using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core;
using TimeTrack.Core.Model;

namespace TimeTrack.UseCase
{
    public class MemberUseCase
    {
        private TimeTrackDbContext _context;
        
        public MemberUseCase(TimeTrackDbContext context)
        {
            _context = context;
        }

        public async Task<UseCaseResult<MemberEntity>> GetAllAsync()
        {
            var r = await _context.Members.ToListAsync();
            
            return UseCaseResult<MemberEntity>.Success(r);
        }

        public async Task<UseCaseResult<MemberEntity>> GetSingleAsync(int id)
        {
            var r = await _context.Members.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.NotFound, new { Id = id });
            }
            
            return UseCaseResult<MemberEntity>.Success(r);
        }

        public async Task<UseCaseResult<MemberEntity>> SetPassword(int id, string password)
        {
            var member = await _context.Members.SingleOrDefaultAsync(x => x.Id == id);

            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Das Mitglied existiert nicht."
                });
            }
            
            member.SetPassword(password);
            await _context.SaveChangesAsync();
            return UseCaseResult<MemberEntity>.Success(member);
        }

        public async Task<UseCaseResult<MemberEntity>> PutSingleAsync(MemberEntity member)
        {
            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Das Mitglied ist fehlerhaft!"
                });
            }

            if (string.IsNullOrWhiteSpace(member.Mail))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die E-Mail fehlt!"
                });
            }
            
            if (member.Mail.Length > 320)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die E-Mail ist zu lang!"
                });
            }

            if (await _context.Members.AnyAsync(x => x.Mail == member.Mail))
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die E-Mail wird schon verwendet!"
                });
            }
            
            
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            
            return UseCaseResult<MemberEntity>.Success(member);
        }

        public async Task<UseCaseResult<MemberEntity>> DeleteSingleAsync(int id)
        { 
            var r = await _context.Members.Include(x => x.Activities).SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.NotFound, new { ID = id });
            }
            
            _context.Activities.RemoveRange(r.Activities);
            _context.Members.Remove(r);
            await _context.SaveChangesAsync();

            return UseCaseResult<MemberEntity>.Success(r);
        }

        public async Task<UseCaseResult<MemberEntity>> UpdateSingleAsync(int id, MemberEntity member)
        {
            if (member == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Das Mitglied ist fehlerhaft!"
                });
            }
            
            var m = await _context.Members.SingleOrDefaultAsync(x => x.Id == id);

            if (m == null)
            {
                return UseCaseResult<MemberEntity>.Failure(UseCaseResultType.NotFound, new { Id = id });
            }

            m.Surname = member.Surname;
            m.GivenName = member.GivenName;
            m.Mail = member.Mail;
            m.Active = member.Active;
            m.MailConfirmed = member.MailConfirmed;
            m.RenewPassword = member.RenewPassword;
            m.Created = member.Created;
            await _context.SaveChangesAsync();

            return UseCaseResult<MemberEntity>.Success(m);
        }
    }
}