using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrack.Core;
using TimeTrack.Core.Model;
using TimeTrack.Core.UseCase;

namespace TimeTrack.UseCase
{
    public class ProjectUseCase : IProjectUseCase
    {
        private ITimeTrackDbContext _context;
        private ILogger<ProjectUseCase> _logger;
        
        public ProjectUseCase(ITimeTrackDbContext context)
        {
            _context = context;
        }

        public async Task<UseCaseResult<ProjectEntity>> GetAllAsync()
        {
            var r = await _context.Projects.ToListAsync();

            return UseCaseResult<ProjectEntity>.Success(r);
        }

        public async Task<UseCaseResult<ProjectEntity>> GetSingleAsync(int id)
        {
            var r = await _context.Projects.SingleOrDefaultAsync(x => x.Id == id);
            if (r == null)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.NotFound, new {});
            }
            return UseCaseResult<ProjectEntity>.Success(r);
        }

        public async Task<UseCaseResult<ProjectEntity>> UpdateSingleAsync(int id, ProjectEntity projectEntity)
        {
            if (projectEntity == null)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Das Projekt ist fehlerhaft!"
                });
            }
            
            if (string.IsNullOrWhiteSpace(projectEntity.Name))
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new {});
            }

            if (projectEntity.Name.Length > 100)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new {});
            }

            projectEntity.Name = projectEntity.Name.Trim();
            
            if (await _context.Projects.CountAsync(x => x.Name == projectEntity.Name) == 1)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.Conflict, new {});
            }
            
            var project = await _context.Projects.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (project == null)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.NotFound, new {Id = id});
            }

            project.Name = projectEntity.Name;

            await _context.SaveChangesAsync();

            return UseCaseResult<ProjectEntity>.Success(project);
        }

        public async Task<UseCaseResult<ProjectEntity>> CreateSingleAsync(ProjectEntity projectEntity)
        {
            if (projectEntity == null)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Das Projekt ist fehlerhaft!"
                });
            }

            if (string.IsNullOrWhiteSpace(projectEntity.Name))
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name fehlt!"
                });
            }

            if (projectEntity.Name.Length > 100)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Name ist länger als 100 Zeichen!"
                });
            }
            
            projectEntity.Name = projectEntity.Name.Trim();
            projectEntity.Id = 0;
            
            var exists = await _context.Projects.AsNoTracking().AnyAsync(x => x.Name == projectEntity.Name);
            
            if (exists)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.Conflict, new {Name=projectEntity.Name, Message="Ein Projekt mit dem gleichen Namen existiert bereits!"});
            }
            
            await _context.Projects.AddAsync(projectEntity);
            await _context.SaveChangesAsync();

            return UseCaseResult<ProjectEntity>.Success(projectEntity);
        }

        public async Task<UseCaseResult<ProjectEntity>> DeleteSingleAsync(int id)
        {
            if (await _context.Projects.CountAsync() == 1)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.Conflict, new
                {
                    Message="Es kann nicht der letzte Datensatz gelöscht werden!"
                });
            }

            var r = await _context.Projects.Include(x => x.Activities).SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }

            if (r.Activities != null && r.Activities.Count > 0)
            {
                return UseCaseResult<ProjectEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der Datensatz kann nicht gelöscht werden, weil der noch verwendet wird."
                });
            }
            
            _context.Projects.Remove(r);
            await _context.SaveChangesAsync();
            
            return UseCaseResult<ProjectEntity>.Success(r);
        }
    }
}
