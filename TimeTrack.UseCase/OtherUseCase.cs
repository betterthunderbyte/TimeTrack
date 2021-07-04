using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Core.UseCase;

namespace TimeTrack.UseCase
{
    public class OtherUseCase : IOtherUseCase
    {
        private ITimeTrackDbContext _context;
        
        public OtherUseCase(ITimeTrackDbContext context)
        {
            _context = context;
        }

        public async Task<UseCaseResult<FullDataTransfer>> GetFullFromUserAsync(int ownerId)
        {
            var activities = (await _context.Activities.Where(x => x.OwnerFk == ownerId).ToArrayAsync()).OrderByDescending(x => x.Begin).ToList();
            var activitiesConverted = activities.ConvertAll(x =>
            {
                var ac = new ActivityDataTransfer();
                ac.From(x);
                return ac;
            });

            var activityTypes = await _context.ActivityTypes.ToListAsync();
            var activityTypesConverted = activityTypes.ConvertAll(x =>
            {
                var atdt = new ActivityTypeDataTransfer();
                atdt.From(x);
                return atdt;
            });

            var projects = await _context.Projects.ToListAsync();
            var projectsConverted = projects.ConvertAll(x =>
            {
                var pdt = new ProjectDataTransfer();
                pdt.From(x);
                return pdt;
            });

            var customers = await _context.Customers.ToListAsync();
            var customersConverted = customers.ConvertAll(x =>
            {
                var cdt = new CustomerDataTransfer();
                cdt.From(x);
                return cdt;
            });

            var r = new FullDataTransfer()
            {
                Activities = activitiesConverted,
                ActivityTypes = activityTypesConverted,
                Projects = projectsConverted,
                Customers = customersConverted
            };
            
            return UseCaseResult<FullDataTransfer>.Success(r);
        }
    }
}