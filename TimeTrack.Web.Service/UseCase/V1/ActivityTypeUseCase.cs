using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core;
using TimeTrack.Db;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.UseCase.V1
{
    public class ActivityTypeUseCase
    {
        TimeTrackDbContext _timeTrackDbContext;

        public ActivityTypeUseCase(TimeTrackDbContext timeTrackDbContext)
        {
            _timeTrackDbContext = timeTrackDbContext;
        }

        public async Task<UseCaseResult<ActivityTypeEntity>> GetAllAsync()
        {
            var r = await _timeTrackDbContext.ActivityTypes.ToListAsync();
            return UseCaseResult<ActivityTypeEntity>.Success(r);
        }

        public async Task<UseCaseResult<ActivityTypeEntity>> GetSingleAsync(int id)
        {
            if (id < 1)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.NotFound, new {Id = id, Message="Der Datensatz mit der Id konnte nicht gefunden werden!"});
            }

            var r = await _timeTrackDbContext.ActivityTypes.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (r == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Id = id, 
                    Message = "Der Datensatz mit der Id konnte nicht gefunden werden!"
                });
            }
            
            return UseCaseResult<ActivityTypeEntity>.Success(r);
        }
        
        public async Task<UseCaseResult<ActivityTypeEntity>> CreateSingleAsync(ActivityTypeEntity activityTypeEntity)
        {
            if (activityTypeEntity == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der  Tätigkeitstyp ist fehlerhaft!"
                });
            }

            
            if (activityTypeEntity.Title == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new {Message="Der Titel fehlt!", Title = activityTypeEntity.Title});
            }
            
            if (activityTypeEntity.Title.Length > 100)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new {Message="Der Titel ist länger als 100 Zeichen!", Title = activityTypeEntity.Title});
            }
            
            if (string.IsNullOrWhiteSpace(activityTypeEntity.Title))
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new {Message="Die Tätigkeit wurde nicht angegeben!", Title = "?"});
            }

            if (activityTypeEntity.Description != null && activityTypeEntity.Description.Length > 250)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new {Message="Die Beschreibung ist länger als 250 Zeichen!", Description = activityTypeEntity.Description});
            }

            activityTypeEntity.Title = activityTypeEntity.Title.Trim();
            
            if (await _timeTrackDbContext.ActivityTypes.CountAsync(x => x.Title == activityTypeEntity.Title) > 0)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.Conflict, new {Message="Die Tätigkeit existiert bereits!", Title = activityTypeEntity.Title});
            }

            activityTypeEntity.Id = 0;
            
            await _timeTrackDbContext.ActivityTypes.AddAsync(activityTypeEntity);
            await _timeTrackDbContext.SaveChangesAsync();
            return UseCaseResult<ActivityTypeEntity>.Success(activityTypeEntity);
        }

        public async Task<UseCaseResult<ActivityTypeEntity>> UpdateSingleAsync(int id,
            ActivityTypeEntity activityTypeEntity)
        {
            if (activityTypeEntity == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Der  Tätigkeitstyp ist fehlerhaft!"
                });
            }

            if (activityTypeEntity.Title != null && activityTypeEntity.Title.Length > 100)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.BadRequest, new {Message="Der Titel ist länger als 100 Zeichen!", Title = activityTypeEntity.Title});
            }
            
            var r = await _timeTrackDbContext.ActivityTypes.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz konnte nicht gefunden werden!"
                });
            }
            
            if (activityTypeEntity.Title != null)
            {
                activityTypeEntity.Title = activityTypeEntity.Title.Trim();
                if (activityTypeEntity.Title.Length > 0)
                {
                    if (await _timeTrackDbContext.ActivityTypes.CountAsync(x => x.Title == activityTypeEntity.Title) > 0)
                    {
                        return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.Conflict, new {Message="Die Tätigkeit exisitert bereits.", Title=activityTypeEntity.Title});
                    }
 
                    r.Title = activityTypeEntity.Title;
                }
            }

            r.Description = activityTypeEntity.Description ?? r.Description;
            
            await _timeTrackDbContext.SaveChangesAsync();
            return UseCaseResult<ActivityTypeEntity>.Success(r);    
        }

        public async Task<UseCaseResult<ActivityTypeEntity>> DeleteSingleAsync(int id)
        {
            if (await _timeTrackDbContext.ActivityTypes.CountAsync() == 1)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.Conflict, new
                {
                    Message="Der letzte Datensatz kann nicht gelöscht werden!"
                });
            }
            
            var r = await _timeTrackDbContext.ActivityTypes.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ActivityTypeEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz konnte nicht gefunden werden!", Id = id
                });
            }
            
            _timeTrackDbContext.ActivityTypes.Remove(r);
            await _timeTrackDbContext.SaveChangesAsync();
            return UseCaseResult<ActivityTypeEntity>.Success(r);
        }
    }
}