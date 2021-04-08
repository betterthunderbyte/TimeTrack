using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Db;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.UseCase.V1
{
    public class ActivityUseCase
    {
        TimeTrackDbContext _timeTrackDbContext;

        public ActivityUseCase(TimeTrackDbContext timeTrackDbContext)
        {
            _timeTrackDbContext = timeTrackDbContext;
        }

        // ToDo UnitTest
        public async Task<UseCaseResult<ActivityEntity>> GetAllFromUserAsync(int userId)
        {
            var r = await _timeTrackDbContext.Activities.Where(x => x.OwnerFk == userId).ToListAsync();
            
            return UseCaseResult<ActivityEntity>.Success(r);
        }

        public async Task<UseCaseResult<WeekDurationDataTransfer>> GetDurationFromWeekInDaysFromOwner(int userId, DateTimeOffset dateTime)
        {
            var monday = dateTime.GetMondayInThisWeek();
            var tuesday = dateTime.GetTuesdayInThisWeek();
            var wednesday = dateTime.GetWednesdayInThisWeek();
            var thursday = dateTime.GetThursdayInThisWeek();
            var friday = dateTime.GetFridayInThisWeek();
            var saturday = dateTime.GetSaturdayInThisWeek();
            var sunday = dateTime.GetSundayInThisWeek();

            var activities = await _timeTrackDbContext.Activities.Where(x => x.OwnerFk == userId).ToListAsync();

            var mondayTicks = activities.Where(x => x.Begin >= monday.GetStart() && x.Begin <= monday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var mondayDuration = new TimeDataTransfer();
            mondayDuration.From(new TimeSpan(mondayTicks));
            
            var tuesdayTicks = activities.Where(x => x.Begin >= tuesday.GetStart() && x.Begin <= tuesday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var tuesdayDuration = new TimeDataTransfer();
            tuesdayDuration.From(new TimeSpan(tuesdayTicks));
            
            var wednesdayTicks = activities.Where(x => x.Begin >= wednesday.GetStart() && x.Begin <= wednesday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var wednesdayDuration = new TimeDataTransfer();
            wednesdayDuration.From(new TimeSpan(wednesdayTicks));
            
            var thursdayTicks = activities.Where(x => x.Begin >= thursday.GetStart() && x.Begin <= thursday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var thursdayDuration = new TimeDataTransfer();
            thursdayDuration.From(new TimeSpan(thursdayTicks));
            
            var fridayTicks = activities.Where(x => x.Begin >= friday.GetStart() && x.Begin <= friday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var fridayDuration = new TimeDataTransfer();
            fridayDuration.From(new TimeSpan(fridayTicks));
            
            var saturdayTicks = activities.Where(x => x.Begin >= saturday.GetStart() && x.Begin <= saturday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var saturdayDuration = new TimeDataTransfer();
            saturdayDuration.From(new TimeSpan(saturdayTicks));
            
            var sundayTicks = activities.Where(x => x.Begin >= sunday.GetStart() && x.Begin <= sunday.GetEnd())
                .Sum(x => x.Duration.Ticks);
            var sundayDuration = new TimeDataTransfer();
            sundayDuration.From(new TimeSpan(sundayTicks));
            
            return UseCaseResult<WeekDurationDataTransfer>.Success(new WeekDurationDataTransfer()
            {
                Monday = mondayDuration,
                Tuesday = tuesdayDuration,
                Wednesday = wednesdayDuration,
                Thursday = thursdayDuration,
                Friday = fridayDuration,
                Saturday = saturdayDuration,
                Sunday = sundayDuration
            });
        }
         
        public async Task<UseCaseResult<ActivityEntity>> GetAllAsync()
        {
            var r = (await _timeTrackDbContext.Activities.ToArrayAsync()).ToList();
            return UseCaseResult<ActivityEntity>.Success(r);
        }
        
        public async Task<UseCaseResult<ActivityEntity>> GetSingleAsync(int id)
        {
            var entity = await _timeTrackDbContext.Activities.SingleOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz konnte nicht gefunden werden!"
                });
            }
            
            return UseCaseResult<ActivityEntity>.Success(entity);
        }

        public async Task<UseCaseResult<ActivityEntity>> CreateSingleAsync(ActivityEntity activityEntity)
        {
            if (activityEntity == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die Tätigkeit ist fehlerhaft!"
                });
            }
            
            if (await _timeTrackDbContext.ActivityTypes.CountAsync(x => x.Id == activityEntity.ActivityTypeFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Die Tätigkeit existiert nicht!", 
                    Id = activityEntity.ActivityTypeFk
                });
            }
            
            if (await _timeTrackDbContext.Customers.CountAsync(x => x.Id == activityEntity.CustomerFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Kunde existiert nicht!", 
                    Id = activityEntity.CustomerFk
                });
            }
            
            if (await _timeTrackDbContext.Projects.CountAsync(x => x.Id == activityEntity.ProjectFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Das Projekt existiert nicht!", 
                    Id = activityEntity.ProjectFk
                });
            }
            
            if (await _timeTrackDbContext.Members.CountAsync(x => x.Id == activityEntity.OwnerFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Das Mitglied existiert nicht!", 
                    Id = activityEntity.OwnerFk
                });
            }

            activityEntity.Begin = new DateTimeOffset(
                activityEntity.Begin.Year,
                activityEntity.Begin.Month,
                activityEntity.Begin.Day,
                activityEntity.Begin.Hour,
                activityEntity.Begin.Minute,
                0,
                activityEntity.Begin.Offset
            );

            activityEntity.Duration = new TimeSpan(
                activityEntity.Duration.Hours,
                activityEntity.Duration.Minutes,
                0
            );
            
            // ToDo Unit Test 
            // Wenn die Zeit länger ist als ein Tag, dann abziehen, bis es passt
            var time = activityEntity.Begin.TimeOfDay + activityEntity.Duration;
            if (time > TimeSpan.FromDays(1))
            {
                activityEntity.Duration = activityEntity.Duration.Subtract(time - TimeSpan.FromDays(1));
            }
            
            await ModifyAllActivitiesConflictWith(activityEntity);
            await _timeTrackDbContext.Activities.AddAsync(activityEntity);
            
            await _timeTrackDbContext.SaveChangesAsync();

            return UseCaseResult<ActivityEntity>.Success(activityEntity);
        }

        public async Task<UseCaseResult<ActivityEntity>> DeleteSingleAsync(int id)
        {
            var r = await _timeTrackDbContext.Activities.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }

            _timeTrackDbContext.Activities.Remove(r);
            await _timeTrackDbContext.SaveChangesAsync();
            
            return UseCaseResult<ActivityEntity>.Success(r);
        }
        
        // ToDo UnitTest
        public async Task<UseCaseResult<ActivityEntity>> DeleteSingleFromUserAsync(int userId, int id)
        {
            var r = await _timeTrackDbContext.Activities.SingleOrDefaultAsync(x => x.OwnerFk == userId && x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }

            _timeTrackDbContext.Activities.Remove(r);
            await _timeTrackDbContext.SaveChangesAsync();
            
            return UseCaseResult<ActivityEntity>.Success(r);
        }

        // ToDo UnitTest
        public async Task<UseCaseResult<ActivityEntity>> UpdateSingleFromUserAsync(int userId, int id, ActivityEntity activityEntity)
        {
            if (activityEntity == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die Tätigkeit ist fehlerhaft!"
                });
            }
            
            if (await _timeTrackDbContext.Activities.CountAsync(x => x.OwnerFk == userId && x.Id == id) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }

            activityEntity.OwnerFk = userId;
            
            return await UpdateSingleAsync(id, activityEntity);
        }
        
        public async Task<UseCaseResult<ActivityEntity>> UpdateSingleAsync(int id, ActivityEntity activityEntity)
        {
            if (activityEntity == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.BadRequest, new
                {
                    Message="Die Tätigkeit ist fehlerhaft!"
                });
            }
            
            if (await _timeTrackDbContext.Activities.CountAsync(x => x.Id == id) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Datensatz existiert nicht!"
                });
            }
            
            if (await _timeTrackDbContext.ActivityTypes.CountAsync(x => x.Id == activityEntity.ActivityTypeFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Die Tätigkeit existiert nicht!", 
                    Id = activityEntity.ActivityTypeFk
                });
            }
            
            if (await _timeTrackDbContext.Customers.CountAsync(x => x.Id == activityEntity.CustomerFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Der Kunde existiert nicht!", 
                    Id = activityEntity.CustomerFk
                });
            }
            
            if (await _timeTrackDbContext.Projects.CountAsync(x => x.Id == activityEntity.ProjectFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Das Projekt existiert nicht!", 
                    Id = activityEntity.ProjectFk
                });
            }
            
            if (await _timeTrackDbContext.Members.CountAsync(x => x.Id == activityEntity.OwnerFk) == 0)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new
                {
                    Message="Das Mitglied existiert nicht!", 
                    Id = activityEntity.OwnerFk
                });
            }
            
            var r = await _timeTrackDbContext.Activities.SingleOrDefaultAsync(x => x.Id == id);

            if (r == null)
            {
                return UseCaseResult<ActivityEntity>.Failure(UseCaseResultType.NotFound, new { Id = id });
            }

            r.CustomerFk = activityEntity.CustomerFk;
            r.OwnerFk = activityEntity.OwnerFk;
            r.ProjectFk = activityEntity.ProjectFk;
            r.ActivityTypeFk = activityEntity.ActivityTypeFk;

            r.Begin = new DateTimeOffset(
                activityEntity.Begin.Year,
                activityEntity.Begin.Month,
                activityEntity.Begin.Day,
                activityEntity.Begin.Hour,
                activityEntity.Begin.Minute,
                0,
                activityEntity.Begin.Offset
            );
            
            r.Duration = new TimeSpan(
                activityEntity.Duration.Hours,
                activityEntity.Duration.Minutes,
                0
            );
            
            var time = activityEntity.Begin.TimeOfDay + activityEntity.Duration - TimeSpan.FromDays(1);
            activityEntity.Duration = activityEntity.Duration.Subtract(time);
            
            await _timeTrackDbContext.SaveChangesAsync();
            
            return UseCaseResult<ActivityEntity>.Success(r);
        }

        private async Task ModifyAllActivitiesConflictWith(ActivityEntity activityEntity)
        {
            if (activityEntity == null)
            {
                return;
            }
            
            var max = activityEntity.Begin + activityEntity.Duration;
            var min = activityEntity.Begin;

            var activityEntities = (await _timeTrackDbContext.Activities.Where(x => x.OwnerFk == activityEntity.OwnerFk)
                .ToArrayAsync())
                .Where(x => x.Begin + x.Duration >= min && x.Begin <= max);
            
            // 
            /*
            var activityEntities = (await _timeTrackDbContext.Activities.Where(x =>
                x.OwnerFk == activityEntity.OwnerFk && (x.Begin >= min && x.Begin <= max)).ToArrayAsync());
            */        
            
            // ToDo Aufteilen u. Unit Tests
            foreach (var entity in activityEntities)
            {
                if (activityEntity.Begin <= entity.Begin &&
                    activityEntity.Begin + activityEntity.Duration > entity.Begin)
                {
                    // Case 1
                    var value = activityEntity.Begin + activityEntity.Duration - entity.Begin;
                    entity.Begin += value;
                    entity.Duration = TimeSpan.Zero;
                    //entity.Duration -= value;
                }
                else if (activityEntity.Begin > entity.Begin &&
                         activityEntity.Begin + activityEntity.Duration < entity.Begin + entity.Duration)
                {
                    // Case 2    
                    ActivityEntity newOne = new ActivityEntity();
                    newOne.Begin = activityEntity.Begin + activityEntity.Duration;
                    newOne.Duration = entity.Begin + entity.Duration - newOne.Begin;
                    newOne.ActivityTypeFk = entity.ActivityTypeFk;
                    newOne.CustomerFk = entity.CustomerFk;
                    newOne.ProjectFk = entity.ProjectFk;
                    newOne.OwnerFk = entity.OwnerFk;
                    entity.Duration = activityEntity.Begin - entity.Begin;

                    await _timeTrackDbContext.Activities.AddAsync(newOne);
                }
                else if(activityEntity.Begin > entity.Begin && activityEntity.Begin + activityEntity.Duration >= entity.Begin + entity.Duration)
                {
                    // Case 3
                    entity.Duration -= entity.Begin + entity.Duration - activityEntity.Begin;
                }
                else if (activityEntity.Begin == entity.Begin && activityEntity.Duration == entity.Duration)
                {
                    // Case 4 - Überschreiben
                    entity.Duration = TimeSpan.Zero;
                }
               
            }

            await _timeTrackDbContext.SaveChangesAsync();
        }

    }
}